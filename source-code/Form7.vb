Imports MySql.Data.MySqlClient

Public Class Form7
    Private Sub Form7_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Label1.Parent = Me.PictureBox1
        Me.Label2.Parent = Me.PictureBox1

        Dim paymentAmount As Integer = Module1.paymentAmount
        Label2.Text = "$" & paymentAmount.ToString()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Dim frm6 As New Form6
        frm6.Show()
        Me.Close()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Dim userID As Integer = Module1.userID
        Dim paymentAmount As Integer = Module1.paymentAmount

        Dim paymentAmountToInsert As Integer = paymentAmount

        Dim amountToInsert As Integer = paymentAmount * 100


        Dim connectionString As String = "server=mysql-1005c393-blackjack.g.aivencloud.com;userid=avnadmin;password=AVNS__oJbEKB_6W6I2kYTbqG;database=blackjack;port=24455;SslMode=Required"

        Using conn As New MySqlConnection(connectionString)
            Try
                conn.Open()

                Dim insertPaymentQuery As String = "INSERT INTO `payment` (user_id, payment_amount) VALUES (@UserID, @PaymentAmount)"
                Using cmd As New MySqlCommand(insertPaymentQuery, conn)
                    cmd.Parameters.AddWithValue("@UserID", userID)
                    cmd.Parameters.AddWithValue("@PaymentAmount", paymentAmountToInsert)
                    cmd.ExecuteNonQuery()
                End Using

                Dim updateWalletQuery As String = "UPDATE `wallet` SET chips = chips + @AmountToInsert WHERE user_id = @UserID"
                Using cmd As New MySqlCommand(updateWalletQuery, conn)
                    cmd.Parameters.AddWithValue("@AmountToInsert", amountToInsert)
                    cmd.Parameters.AddWithValue("@UserID", userID)
                    cmd.ExecuteNonQuery()
                End Using

                MessageBox.Show("Payment recorded and wallet updated.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

                Dim frm2 As New Form2
                frm2.Show()
                Me.Close()

            Catch ex As Exception
                MessageBox.Show("Error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Finally
                conn.Close()
            End Try
        End Using
    End Sub
End Class
