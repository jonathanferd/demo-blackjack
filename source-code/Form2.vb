Imports MySql.Data.MySqlClient

Public Class Form2
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ' Validate and parse the input
        If Integer.TryParse(TextBox1.Text, Module1.chipsBeingBet) AndAlso Module1.chipsBeingBet > 0 Then
            ' Proceed if the value is a valid number greater than zero
            Dim frm3 As New Form3
            frm3.Show()
            Me.Close()
        Else
            ' Show an error message if the value is zero or invalid
            MessageBox.Show("Please enter a valid number of chips greater than zero.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim frm1 As New Form1
        frm1.Show()
        Me.Close()
    End Sub

    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Label1.Parent = Me.PictureBox1
        Me.Label2.Parent = Me.PictureBox1
        Me.Label3.Parent = Me.PictureBox1
        Me.Label4.Parent = Me.PictureBox1
        Me.Label5.Parent = Me.PictureBox1

        ' Fetch and display the current number of chips from the wallet table
        Dim connectionString As String = "server=mysql-1005c393-blackjack.g.aivencloud.com;userid=avnadmin;password=AVNS__oJbEKB_6W6I2kYTbqG;database=blackjack;port=24455;SslMode=Required"
        Using conn As New MySqlConnection(connectionString)
            Try
                conn.Open()

                Dim userID As Integer = Module1.userID
                Dim selectChipsQuery As String = "SELECT chips FROM `wallet` WHERE user_id = @UserID"
                Using cmd As New MySqlCommand(selectChipsQuery, conn)
                    cmd.Parameters.AddWithValue("@UserID", userID)
                    Dim chips As Object = cmd.ExecuteScalar()
                    If chips IsNot Nothing Then
                        Label5.Text = "Chips: " & chips.ToString()
                    Else
                        Label5.Text = "Chips: 0"
                    End If
                End Using

            Catch ex As Exception
                MessageBox.Show("Error fetching chips: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim connectionString As String = "server=mysql-1005c393-blackjack.g.aivencloud.com;userid=avnadmin;password=AVNS__oJbEKB_6W6I2kYTbqG;database=blackjack;port=24455;SslMode=Required"
        Using conn As New MySqlConnection(connectionString)
            Try
                conn.Open()

                Dim userID As Integer = Module1.userID
                Dim chipsToAdd As Integer = 100
                Dim insertChipsQuery As String = "UPDATE `wallet` SET chips = chips + @ChipsToAdd WHERE user_id = @UserID"

                Using cmd As New MySqlCommand(insertChipsQuery, conn)
                    cmd.Parameters.AddWithValue("@ChipsToAdd", chipsToAdd)
                    cmd.Parameters.AddWithValue("@UserID", userID)
                    cmd.ExecuteNonQuery()
                End Using

                MessageBox.Show("100 chips added to your account.", "Chips Added", MessageBoxButtons.OK, MessageBoxIcon.Information)

                Dim frm5 As New Form5
                frm5.Show()
                Me.Close()

            Catch ex As Exception
                MessageBox.Show("Error adding chips: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Dim frm6 As New Form6
        frm6.Show()
        Me.Close()
    End Sub
End Class
