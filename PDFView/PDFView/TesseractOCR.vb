﻿Imports System.Drawing
Imports System.Windows.Forms

Public Class TesseractOCR

  Public Shared Function OCRImage(ByVal bm As System.Drawing.Image) As String
    OCRImage = ""
    Dim oOCR As New tessnet2.Tesseract
    oOCR.Init(Nothing, "eng", False)
    Dim WordList As New List(Of tessnet2.Word)
    WordList = oOCR.doOCR(bm, Rectangle.Empty)
    Dim LineCount As Integer = tessnet2.Tesseract.LineCount(WordList)
    For i As Integer = 0 To LineCount - 1
      OCRImage &= tessnet2.Tesseract.GetLineText(WordList, i) & vbCrLf
    Next
    oOCR.Dispose()
    ''Debug
    'OCRPaintWordBorders(bm, WordList)
  End Function

  Public Shared Sub OCRPaintWordBorders(ByRef img As System.Drawing.Image, ByVal WordList As List(Of tessnet2.Word))
    If WordList IsNot Nothing Then
      Dim g As Graphics = Graphics.FromImage(img)
      g.DrawImage(img, 0, 0)
      For Each word As tessnet2.Word In WordList
        Dim pen As Pen = New Pen(Color.FromArgb(255, 128, CInt(word.Confidence)))
        g.DrawRectangle(pen, word.Left, word.Top, word.Right - word.Left, word.Bottom - word.Top)
        'For Each c As tessnet2.Character In word.CharList
        '  e.Graphics.DrawRectangle(Pens.BlueViolet, c.Left + panel2.AutoScrollPosition.X, c.Top + panel2.AutoScrollPosition.Y, c.Right - c.Left, c.Bottom - c.Top)
        'Next
      Next
      g.Dispose()
    End If
  End Sub

  Public Shared Function GetPDFIndex(ByRef img As System.Drawing.Image) As List(Of PDFWordIndex)
    GetPDFIndex = New List(Of PDFWordIndex)
    Dim oOCR As New tessnet2.Tesseract
    oOCR.Init(Nothing, "eng", False)
    Dim WordList As New List(Of tessnet2.Word)
    WordList = oOCR.doOCR(img, Rectangle.Empty)
    If WordList IsNot Nothing Then
      Dim g As Graphics = Graphics.FromImage(img)
      g.DrawImage(img, 0, 0)
      For Each word As tessnet2.Word In WordList
        Dim pdfWordIndex As New PDFWordIndex
        pdfWordIndex.X = word.Left
        pdfWordIndex.Y = word.Top
        pdfWordIndex.Width = word.Right - word.Left
        pdfWordIndex.Height = word.Bottom - word.Top
        pdfWordIndex.FontSize = word.PointSize
        pdfWordIndex.Text = word.Text
        GetPDFIndex.Add(pdfWordIndex)
      Next
    End If
  End Function

End Class

Public Class PDFWordIndex
  Public X As Integer
  Public Y As Integer
  Public Width As Integer
  Public Height As Integer
  Public FontSize As Integer
  Public Text As String
End Class