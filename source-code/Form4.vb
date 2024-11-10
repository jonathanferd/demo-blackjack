Imports MySql.Data.MySqlClient

Public Class Form4
    Dim conn As MySqlConnection
    Dim connectionString As String = "server=mysql-1005c393-blackjack.g.aivencloud.com;userid=avnadmin;password=AVNS__oJbEKB_6W6I2kYTbqG;database=blackjack;port=24455;SslMode=Required"

    Private Sub Form4_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        conn = New MySqlConnection(connectionString)
        Me.Label1.Parent = Me.PictureBox1
        Me.Label2.Parent = Me.PictureBox1
        Me.Label3.Parent = Me.PictureBox1
        Me.Label4.Parent = Me.PictureBox1
        Try
            conn.Open()
            conn.Close()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim email As String = TextBox1.Text
        Dim password As String = TextBox2.Text
        Dim name As String = TextBox3.Text

        Using connection As New MySqlConnection(connectionString)
            Dim commandText As String = "INSERT INTO `user` (`email`, `password`, `username`) VALUES (@Email, @Password, @Name)"
            Dim command As New MySqlCommand(commandText, connection)
            command.Parameters.AddWithValue("@Email", email)
            command.Parameters.AddWithValue("@Password", password)
            command.Parameters.AddWithValue("@Name", name)

            Try
                connection.Open()

                Dim transaction As MySqlTransaction = connection.BeginTransaction()

                command.Transaction = transaction

                Dim rowsAffected As Integer = command.ExecuteNonQuery()

                Dim userID As Integer = CInt(command.LastInsertedId)

                Dim walletCommandText As String = "INSERT INTO `wallet` (`user_id`, `chips`) VALUES (@UserID, 1000)"
                Dim walletCommand As New MySqlCommand(walletCommandText, connection, transaction)
                walletCommand.Parameters.AddWithValue("@UserID", userID)
                walletCommand.ExecuteNonQuery()

                transaction.Commit()

                MessageBox.Show("User account and wallet created successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Catch ex As Exception
                MessageBox.Show("Error inserting record: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try

            Dim frm1 As New Form1
            frm1.Show()
            Me.Close()

        End Using
    End Sub
End Class
