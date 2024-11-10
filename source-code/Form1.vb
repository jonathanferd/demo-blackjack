Imports MySql.Data.MySqlClient

Public Class Form1
    Dim conn As MySqlConnection
    Dim COMMAND As MySqlCommand
    Dim connectionString As String = "server=mysql-1005c393-blackjack.g.aivencloud.com;userid=avnadmin;password=AVNS__oJbEKB_6W6I2kYTbqG;database=blackjack;port=24455;SslMode=Required"

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
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

        Using connection As New MySqlConnection(connectionString)
            Dim command As New MySqlCommand("SELECT user_id FROM `user` WHERE email = @Email AND password = @Password", connection)
            command.Parameters.AddWithValue("@Email", email)
            command.Parameters.AddWithValue("@Password", password)

            connection.Open()
            Dim reader As MySqlDataReader = command.ExecuteReader()

            If reader.HasRows Then
                reader.Read()
                Module1.userID = reader.GetInt32("user_id")
                MessageBox.Show("Login berhasil", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Dim frm2 As New Form2
                frm2.Show()
                Me.Close()
            Else
                MessageBox.Show("Login gagal", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If

            reader.Close()
        End Using
    End Sub

    Private Sub Label4_Click(sender As Object, e As EventArgs) Handles Label4.Click
        Dim frm4 As New Form4
        frm4.Show()
        Me.Close()
    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
    End Sub
End Class
