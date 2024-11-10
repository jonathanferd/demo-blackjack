Imports System.Resources

Public Class Form5
    Dim rnd As New Random()
    Dim rm As New ResourceManager("blackjack.Resource1", GetType(Form5).Assembly)

    Private Sub Form5_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Label1.Parent = Me.PictureBox1
        LoadRandomImage()
    End Sub

    Private Sub LoadRandomImage()
        Dim randomNumber As Integer = rnd.Next(55, 61)

        Dim resourceName As String = "_"
        If randomNumber < 10 Then
            resourceName &= "0"
        End If
        resourceName &= randomNumber.ToString()

        Dim image As Image = DirectCast(rm.GetObject(resourceName), Image)

        PictureBox2.Image = image
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim frm2 As New Form2
        frm2.Show()
        Me.Close()
    End Sub
End Class
