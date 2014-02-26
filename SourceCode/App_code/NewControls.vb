Imports System.ComponentModel
'Customize save attribute by overise ViewState
Namespace NewControls
    <DefaultProperty("Text")> _
    <ToolboxData("<{0}:ServerControl1 runat=server></{0}:ServerControl1>")> _
    Public Class NewDropDownList
        Inherits DropDownList

        <Bindable(True)> _
        <Category("Appearance")> _
        <DefaultValue("")> _
        <Localizable(True)> _
        Protected Overrides Function SaveViewState() As Object
            ' create object array for Item count + 1
            Dim allStates As Object() = New Object(Me.Items.Count) {}

            ' the +1 is to hold the base info
            Dim baseState As Object = MyBase.SaveViewState()
            allStates(0) = baseState

            Dim i As Int32 = 1
            ' now loop through and save each Style attribute for the List
            For Each li As ListItem In Me.Items
                Dim j As Int32 = 0
                Dim attributes As String()() = New String(li.Attributes.Count - 1)() {}
                For Each attribute As String In li.Attributes.Keys
                    attributes(j) = New String() {attribute, li.Attributes(attribute)}
                    j = j + 1
                   Next
                allStates(i) = attributes
                i = i + 1

            Next
            Return allStates
        End Function

        Protected Overrides Sub LoadViewState(ByVal savedState As Object)
            If savedState IsNot Nothing Then
                Dim myState As Object() = DirectCast(savedState, Object())
                ' restore base first
                If myState(0) IsNot Nothing Then
                    MyBase.LoadViewState(myState(0))
                End If
                Dim i As Int32 = 1
                For Each li As ListItem In Me.Items
                    For Each attribute As String() In DirectCast(myState(i), String()())
                        li.Attributes(attribute(0)) = attribute(1)
                    Next
                    i = i + 1
                Next
            End If
        End Sub
    End Class
End Namespace
