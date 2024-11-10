Imports System.Resources
Imports MySql.Data.MySqlClient
Imports System.Threading.Tasks

Public Class Form3
    Dim deck As New List(Of Integer)
    Dim playerHand As New List(Of Integer)
    Dim dealerHand As New List(Of Integer)
    Dim rnd As New Random()
    Dim rm As New ResourceManager("blackjack.Resource1", GetType(Form3).Assembly)
    Dim conn As MySqlConnection
    Dim connectionString As String = "server=mysql-1005c393-blackjack.g.aivencloud.com;userid=avnadmin;password=AVNS__oJbEKB_6W6I2kYTbqG;database=blackjack;port=24455;SslMode=Required"

    Private Sub Form3_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        conn = New MySqlConnection(connectionString)
        InitializeDeck()
        DealInitialCards()
    End Sub

    Private Sub InitializeDeck()
        deck.Clear()
        For i As Integer = 0 To 51
            deck.Add(i)
        Next
    End Sub

    Private Function DrawCard() As Integer
        Dim index As Integer = rnd.Next(deck.Count)
        Dim card As Integer = deck(index)
        deck.RemoveAt(index)
        Return card
    End Function

    Private Function GetCardValue(card As Integer) As Integer
        Dim value As Integer = (card Mod 13) + 1
        If value > 10 Then
            value = 10
        End If
        Return value
    End Function

    Private Function CalculateHandValue(hand As List(Of Integer)) As Integer
        Dim value As Integer = 0
        Dim aces As Integer = 0
        For Each card In hand
            Dim cardValue As Integer = GetCardValue(card)
            If cardValue = 1 Then
                aces += 1
            End If
            value += cardValue
        Next
        While aces > 0 And value + 10 <= 21
            value += 10
            aces -= 1
        End While
        Return value
    End Function

    Private Sub DealInitialCards()
        playerHand.Clear()
        dealerHand.Clear()

        playerHand.Add(DrawCard())
        dealerHand.Add(DrawCard())
        playerHand.Add(DrawCard())

        UpdateUI()
    End Sub

    Private Sub ButtonHit_Click(sender As Object, e As EventArgs) Handles Button1.Click
        playerHand.Add(DrawCard())
        UpdateUI()

        If CalculateHandValue(playerHand) > 21 Then
            MessageBox.Show("Player busts! Dealer wins.", "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Information)
            UpdateGameResult("l")
        End If
    End Sub

    Private Async Sub ButtonStand_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Await Task.Delay(1000)

        While CalculateHandValue(dealerHand) < 17
            dealerHand.Add(DrawCard())
            UpdateUI()
            Await Task.Delay(1000)
        End While

        Dim playerScore As Integer = CalculateHandValue(playerHand)
        Dim dealerScore As Integer = CalculateHandValue(dealerHand)

        If dealerScore > 21 OrElse playerScore > dealerScore Then
            MessageBox.Show("Player wins!", "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Information)
            UpdateGameResult("w")
        ElseIf playerScore < dealerScore Then
            MessageBox.Show("Dealer wins!", "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Information)
            UpdateGameResult("l")
        Else
            MessageBox.Show("It's a tie!", "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Information)
            UpdateGameResult("t")
        End If

        DealInitialCards()
    End Sub

    Private Sub UpdateUI()
        Try
            If playerHand.Count > 0 Then PictureBoxPlayer1.Image = DirectCast(rm.GetObject("_" & (playerHand(0) + 1)), Image)
            If playerHand.Count > 1 Then PictureBoxPlayer2.Image = DirectCast(rm.GetObject("_" & (playerHand(1) + 1)), Image)
            If playerHand.Count > 2 Then PictureBoxPlayer3.Image = DirectCast(rm.GetObject("_" & (playerHand(2) + 1)), Image)
            If playerHand.Count > 3 Then PictureBoxPlayer4.Image = DirectCast(rm.GetObject("_" & (playerHand(3) + 1)), Image)
            If playerHand.Count > 4 Then PictureBoxPlayer5.Image = DirectCast(rm.GetObject("_" & (playerHand(4) + 1)), Image)

            If dealerHand.Count > 0 Then PictureBoxDealer1.Image = DirectCast(rm.GetObject("_" & (dealerHand(0) + 1)), Image)
            If dealerHand.Count > 1 Then PictureBoxDealer2.Image = DirectCast(rm.GetObject("_" & (dealerHand(1) + 1)), Image)
            If dealerHand.Count > 2 Then PictureBoxDealer3.Image = DirectCast(rm.GetObject("_" & (dealerHand(2) + 1)), Image)
            If dealerHand.Count > 3 Then PictureBoxDealer4.Image = DirectCast(rm.GetObject("_" & (dealerHand(3) + 1)), Image)
            If dealerHand.Count > 4 Then PictureBoxDealer5.Image = DirectCast(rm.GetObject("_" & (dealerHand(4) + 1)), Image)

            LabelPlayerScore.Text = "Player: " & CalculateHandValue(playerHand)
            LabelDealerScore.Text = "Dealer: " & If(dealerHand.Count > 0, CalculateHandValue(dealerHand), "??")
        Catch ex As Exception
            MessageBox.Show($"Error loading images: {ex.Message}", "Image Loading Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub UpdateGameResult(result As Char)
        Dim chipsBet As Integer = Module1.chipsBeingBet
        Dim userID As Integer = Module1.userID

        Try
            conn.Open()

            Dim insertMatchQuery As String = "INSERT INTO `match` (user_id, result) VALUES (@UserID, @Result)"
            Using insertCommand As New MySqlCommand(insertMatchQuery, conn)
                insertCommand.Parameters.AddWithValue("@UserID", userID)
                insertCommand.Parameters.AddWithValue("@Result", result)
                insertCommand.ExecuteNonQuery()
            End Using

            Dim chipsChange As Integer = 0

            If result = "w" Then
                chipsChange = chipsBet
            ElseIf result = "l" Then
                chipsChange = -chipsBet
            End If

            Dim updateChipsQuery As String = "UPDATE `wallet` SET chips = chips + @ChipsChange WHERE user_id = @UserID"
            Using updateCommand As New MySqlCommand(updateChipsQuery, conn)
                updateCommand.Parameters.AddWithValue("@ChipsChange", chipsChange)
                updateCommand.Parameters.AddWithValue("@UserID", userID)
                updateCommand.ExecuteNonQuery()
            End Using

        Catch ex As Exception
            MessageBox.Show("Error updating game result: " & ex.Message)
        Finally
            conn.Close()
        End Try

        Dim frm2 As New Form2
        frm2.Show()
        Me.Close()
    End Sub

End Class
