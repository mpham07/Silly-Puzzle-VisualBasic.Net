' Game  : Silly Puzzle
' Author: Minh Pham
' Date  : 08/04/2017 

Public Class Form1

    Const NOT_FOUND As Long = 100
    Const EMPTY_VALUE As Long = -1
    Const MAX As Long = 14

    Dim LEFT_SIDE_COL() As Integer = {0, 3, 6, 9, 12}
    Dim DIRECTIONS_FOR_LEFT() As Integer = {1, -3, 3} ' Check Right, Up, Down

    Dim RIGHT_SIDE_COL() As Integer = {2, 5, 8, 11, 14}
    Dim DIRECTIONS_FOR_RIGHT() As Integer = {-1, -3, 3} ' Check Left , Up, Down

    Dim DIRECTIONS_FOR_MIDDLE() As Integer = {-1, 1, -3, 3} ' Check Left, Right, Up, Down

    Dim arrayOfPictureBoxValue() As Integer = {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, -1, 13, 14}
    Dim arrayOfMainConstantValue() As Integer = {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, -1, 13, 14}

    Dim isPlaying As Boolean = False
    Dim currentPositionOfEmptyBox As Integer = 12

    Dim milSec As Integer
    Dim sec As Integer
    Dim min As Integer
    Dim isPausing As Boolean = False

    Private Sub initClock()
        milSec = 0
        sec = 0
        min = 0

        timer1.Stop()
        isPausing = False
    End Sub

    Private Function randomNewPostion(ByVal arr() As Integer) As Integer
        Dim randNumber As Integer
        Dim rand As New Random()
        Dim newPosition As Integer

        '1st Loop:
        Do
            randNumber = arr(rand.Next(0, arr.Length))
            newPosition = currentPositionOfEmptyBox + randNumber
        Loop Until newPosition >= 0 And newPosition < 15

        randomNewPostion = newPosition

    End Function

    Private Sub randomGame()
        Dim number As Integer

        '2nd Loop:
        For i = 0 To 1000

            If LEFT_SIDE_COL.Contains(currentPositionOfEmptyBox) Then
                number = randomNewPostion(DIRECTIONS_FOR_LEFT)
            ElseIf RIGHT_SIDE_COL.Contains(currentPositionOfEmptyBox) Then
                number = randomNewPostion(DIRECTIONS_FOR_RIGHT)
            Else
                number = randomNewPostion(DIRECTIONS_FOR_MIDDLE)
            End If

            Dim p1 = findPictureboxByIndex(currentPositionOfEmptyBox)
            Dim p2 = findPictureboxByIndex(number)
            swap(p1, p2)

        Next
    End Sub

    Private Sub startGame()

        initClock()
        randomGame()

        btnPause.Enabled = True
        isPlaying = True
        btnPause.Text = "Pause"
        timer1.Start()
    End Sub

    Private Sub pauseGame()

        If isPausing = False Then
            timer1.Stop()
            isPlaying = False
            isPausing = True
            btnPause.Text = "Resume"
        Else
            timer1.Start()
            isPlaying = True
            isPausing = False
            btnPause.Text = "Pause"
        End If
    End Sub



    Private Sub swap(pic1 As PictureBox, pic2 As PictureBox)

        Dim picTemp As New PictureBox
        picTemp.Image = pic1.Image
        pic1.Image = pic2.Image
        pic2.Image = picTemp.Image

        Dim valueTemp As Integer = arrayOfPictureBoxValue(pic1.Tag)
        arrayOfPictureBoxValue(pic1.Tag) = arrayOfPictureBoxValue(pic2.Tag)
        arrayOfPictureBoxValue(pic2.Tag) = valueTemp

        currentPositionOfEmptyBox = pic2.Tag
    End Sub

    Private Function supportFindEmpty(indexClicked As Integer, ByVal direction() As Integer) As Integer
        Dim result As Integer = NOT_FOUND

        '3rd Loop
        For Each value In direction
            Dim indexDirection = indexClicked + value

            If indexDirection >= 0 And indexDirection < 15 Then
                If arrayOfPictureBoxValue(indexDirection) = EMPTY_VALUE Then
                    result = indexDirection
                End If
            End If
        Next

        supportFindEmpty = result
    End Function

    Private Function findEmpty(indexClicked As Integer) As Integer

        Dim result As Integer = NOT_FOUND

        If LEFT_SIDE_COL.Contains(indexClicked) Then
            result = supportFindEmpty(indexClicked, DIRECTIONS_FOR_LEFT)

        ElseIf RIGHT_SIDE_COL.Contains(indexClicked) Then
            result = supportFindEmpty(indexClicked, DIRECTIONS_FOR_RIGHT)
        Else
            result = supportFindEmpty(indexClicked, DIRECTIONS_FOR_MIDDLE)
        End If

        findEmpty = result
    End Function

    Private Function findPictureboxByIndex(index As Integer) As PictureBox
        Dim picBox As New PictureBox

        '4th Loop:
        For Each pic In panel.Controls

            If TypeOf pic Is PictureBox Then
                If pic.Tag = index Then
                    picBox = pic
                End If
            End If
        Next

        findPictureboxByIndex = picBox
    End Function

    Private Function checkWin() As Boolean
        Dim result As Boolean = True

        '5th Loop:
        For i = 0 To MAX
            If arrayOfPictureBoxValue(i) <> arrayOfMainConstantValue(i) Then
                result = False
            End If
        Next

        checkWin = result
    End Function

    Private Sub pictureBox_Click(sender As Object, e As EventArgs) Handles p9.Click, p8.Click, p7.Click, p6.Click, p5.Click, p4.Click, p3.Click, p2.Click, p14.Click, p13.Click, p12.Click, p11.Click, p10.Click, p1.Click, p0.Click

        If isPlaying = True Then
            Dim pClicked As PictureBox = DirectCast(sender, PictureBox)
            Dim indexEmptyFound As Integer = findEmpty(pClicked.Tag)

            If indexEmptyFound <> NOT_FOUND Then
                swap(pClicked, findPictureboxByIndex(indexEmptyFound))
                If checkWin() Then
                    timer1.Stop()
                    MessageBox.Show("You Done. Yeahhhhh!!")
                    isPlaying = False

                End If
            End If
        End If
    End Sub

    Private Sub btnNewGame_Click(sender As Object, e As EventArgs) Handles btnNewGame.Click

        startGame()
    End Sub

    Private Sub btnPause_Click(sender As Object, e As EventArgs) Handles btnPause.Click

        pauseGame()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles timer1.Tick
        milSec = milSec + 1
        If milSec = 70 Then
            milSec = 0
            sec = sec + 1
            If sec = 60 Then
                sec = 0
                min = min + 1
            End If
        End If

        txtMilSec.Text = addZero(milSec)
        txtSec.Text = addZero(sec)
        txtMin.Text = addZero(min)

    End Sub

    Private Function addZero(digit As Integer) As String
        Dim result As String = digit

        If digit < 9 Then
            result = "0" & digit
        End If
        addZero = result
    End Function

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked

        Dim title As String = "Welcome to Silly Puzzle! "
        Dim mess As String =
        "Author: Minh Pham" & vbNewLine &
        "Email : ducminh.it48@gmail.com" & vbNewLine &
        "Github: github.com/mpham07" & vbNewLine &
        "Date  : 08/04/2017" & vbNewLine &
        "-------" & vbNewLine &
        "Tab New Game button to start." & vbNewLine &
        "Tab arround the missing tiles" & vbNewLine &
        "to swap them." & vbNewLine &
        "-------" & vbNewLine &
        "Have fun!!"

        MessageBox.Show(mess, title, MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub
End Class
