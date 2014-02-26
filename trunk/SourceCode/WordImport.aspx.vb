Imports Novacode
Partial Class WordImport
    Inherits System.Web.UI.Page

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim FolderPath = Server.MapPath("~/Template").ToString
        Using document As DocX = DocX.Load(FolderPath & "\a.docx")

            ' document.ReplaceText("<<noidung>>", "Merry X-Mas")
            ' Dim _format As New Novacode.Formatting
            ' Dim thisPara As Novacode.Paragraph

            'Dim large_dark_formatting As New Formatting
            'large_dark_formatting.Bold = True
            'large_dark_formatting.FontColor = Drawing.Color.Red

            '   document.InsertParagraph("mot hai ba", False, large_dark_formatting)
            '
            '  document.InsertParagraph("bon nam sau")

            ' Dim p As Paragraph = document.InsertParagraph()

            '   p.IndentationFirstLine = 0
            '   p.AppendLine("thịt nữa",


            '      Dim Title As New Formatting
            '    Title.Bold = True
            ' TVTitle.Misc.
            'Dim p As Paragraph = document.Paragraphs(0)
            ''Dim k = p.I

            'p.Append("Title")
            'p.Append(": DocX is an ")
            'p.Append("opensource library.")

            'Dim thisPara As Novacode.Paragraph = document.Paragraphs.Select(
            'thisPara.StyleName = "TVTitle"

            'thisPara.Append("This is level 1"
            Dim p As Paragraph = document.Paragraphs(0)
            p.StyleName = "TVTitle"
            p.Append("Assumptions1aaa")

            'Dim AssumptionsHeading As Paragraph = document.InsertParagraph

            'AssumptionsHeading.StyleName = "TVTitle"

            'AssumptionsHeading.Append("Assumptions")

            'Dim p2 As Paragraph = document.InsertParagraph

            'p2.StyleName = "TVTitle"

            'p2.Append("Assumptions1")


            'p2.stylename = "Heading2"

            'p2.append("This is level 1.1")




            Dim FileName = "b.docx"
            document.SaveAs(FolderPath & "\" & FileName)


        End Using
    End Sub


#Region "Create document style"



    ' Large Dark formatting - for titles



    '' Dark formatting
    '        Formatting dark_formatting = new Formatting();
    '        dark_formatting.Bold = true;
    '        dark_formatting.Size = 12;
    '        dark_formatting.FontColor = Color.Black;

    '' Light formatting
    '        Formatting light_formatting = new Formatting();
    '        light_formatting.Italic = true;
    '        light_formatting.Size = 11;
    '        light_formatting.FontColor = Color.Black;

#End Region

End Class
