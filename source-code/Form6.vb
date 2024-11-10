Public Class Form6
    Private Sub Form6_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Label1.Parent = Me.PictureBox1
        Me.Label2.Parent = Me.PictureBox1
        Me.Label3.Parent = Me.PictureBox1
        Me.Label4.Parent = Me.PictureBox1
        Me.Label5.Parent = Me.PictureBox1
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Dim frm2 As New Form2
        frm2.Show()
        Me.Close()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Module1.paymentAmount = 1
        Dim frm7 As New Form7
        frm7.Show()
        Me.Close()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Module1.paymentAmount = 10
        Dim frm7 As New Form7
        frm7.Show()
        Me.Close()

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Module1.paymentAmount = 50
        Dim frm7 As New Form7
        frm7.Show()
        Me.Close()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Module1.paymentAmount = 100
        Dim frm7 As New Form7
        frm7.Show()
        Me.Close()
    End Sub
End Class