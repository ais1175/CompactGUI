﻿Class HomePage

    Private _viewModel As HomeViewModel


    Sub New(viewmodel As HomeViewModel)

        ' This call is required by the designer.
        InitializeComponent()
        _viewModel = viewmodel
        DataContext = viewmodel

        ScrollViewer.SetCanContentScroll(Me, False)

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Async Sub AddFolderButton_Click(sender As Object, e As RoutedEventArgs) Handles BtnAddFolder1.Click, BtnAddFolder2.Click
        Dim folderBrowser As New Microsoft.Win32.OpenFolderDialog With {
            .Title = "Select a folder to compress",
            .Multiselect = True,
            .ValidateNames = True
        }
        folderBrowser.ShowDialog()

        If folderBrowser.FolderNames.Length > 0 Then
            Await _viewModel.AddFoldersAsync(folderBrowser.FolderNames)
        End If
    End Sub

    Private Sub Root_DragOver(sender As Object, e As DragEventArgs)

        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            Dim paths As String() = e.Data.GetData(DataFormats.FileDrop)

            If paths.All(Function(path) IO.Directory.Exists(path)) Then
                e.Effects = DragDropEffects.Copy
            Else
                e.Effects = DragDropEffects.None
            End If


        Else
            e.Effects = DragDropEffects.None
        End If

        e.Handled = True

    End Sub

    Private Sub Root_Drop(sender As Object, e As DragEventArgs)

        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            Dim paths As String() = e.Data.GetData(DataFormats.FileDrop)
            If paths.All(Function(path) IO.Directory.Exists(path)) Then
                _viewModel.AddFoldersAsync(paths).ConfigureAwait(False)
            End If
        End If

    End Sub
End Class
