Imports System.Text.RegularExpressions

Namespace IA

    Public Class NLP

        Private endingsList As List(Of String)
        Private pluralList, Extras, ExceptionsList, singularList As String(,)
        Private Max As Integer = 0
        Private Max2 As Integer = 0
        Private Max3 As Integer = 0

        Public Sub New(lang As String)
            PopulateList(lang.ToUpper())
        End Sub

        Public Function Stem(ByRef stringText As String) As String

            Dim result As String = RemoveEspecials(stringText).ToLower()

            For index As Integer = 0 To endingsList.Count - 1
                If result.EndsWith(endingsList(index)) Then
                    result = stringText.Substring(0, stringText.Length - endingsList(index).Length)
                    Exit For
                End If
            Next

            Return result

        End Function

        Public Function WordTokenize(ByRef stringText As String) As String()
            If (stringText.Contains(" ")) Then
                Return Regex.Split(stringText, " ")
            Else
                Return New String() {stringText}
            End If
        End Function

        Public Function WordsLike(ByRef word1 As String, word2 As String) As Single
            Return StartCompare(word1, word2)
        End Function

        Public Function Singularize(ByRef stringText As String) As String
            Try
                Dim result As String = stringText
                Dim textprocess = stringText.Split(" ")

                For txt As Integer = 0 To textprocess.Length - 1
                    If (textprocess(txt).Trim.Length > 0) Then
                        Dim skip As Boolean = False

                        For index As Integer = 0 To Max2
                            If (textprocess(txt).Contains(ExceptionsList(index, 0))) Then
                                textprocess(txt) = textprocess(txt).Replace(ExceptionsList(index, 0), ExceptionsList(index, 1))
                                skip = True
                                Exit For
                            End If
                        Next

                        If (skip) Then
                            Continue For
                        End If

                        For index As Integer = 0 To Max3
                            If textprocess(txt).EndsWith(singularList(index, 1)) Then
                                textprocess(txt) = textprocess(txt).Replace(singularList(index, 1), singularList(index, 0))
                                Exit For
                            End If
                        Next
                        For index As Integer = 0 To Max2
                            If (textprocess(txt).EndsWith("s")) Then
                                textprocess(txt) = textprocess(txt).Substring(0, textprocess(txt).Length - 1)
                            End If
                        Next
                    End If
                Next

                Return String.Join(" ", textprocess)
            Catch ex As Exception
                Return stringText
            End Try
        End Function

        Public Function Pluralize(ByRef stringText As String) As String
            Try
                Dim result As String = stringText
                Dim textprocess = stringText.Split(" ")

                For txt As Integer = 0 To textprocess.Length - 1
                    If (textprocess(txt).Trim.Length > 0) Then
                        Dim skip As Boolean = False

                        For index As Integer = 0 To Max2
                            If (textprocess(txt).Contains(ExceptionsList(index, 0))) Then
                                textprocess(txt) = textprocess(txt).Replace(ExceptionsList(index, 0), ExceptionsList(index, 1))
                                skip = True
                                Exit For
                            End If
                        Next

                        If (skip) Then
                            Continue For
                        End If

                        For index As Integer = 0 To Max
                            If textprocess(txt).EndsWith(pluralList(index, 0)) Then
                                textprocess(txt) = textprocess(txt).Replace(pluralList(index, 0), pluralList(index, 1))
                                Exit For
                            End If
                        Next
                        For index As Integer = 0 To Max2
                            If (Not textprocess(txt).EndsWith("s") And Not textprocess(txt).EndsWith("x")) Then
                                textprocess(txt) = textprocess(txt) + "s"
                            End If
                        Next
                    End If
                Next

                Return String.Join(" ", textprocess)
            Catch ex As Exception
                Return stringText
            End Try
        End Function

#Region "Privates"

        Private Sub PopulateList(ByRef lang As String)
            If (lang = "PT") Then
                Max = 12
                Max2 = 13
                Max3 = 7

                pluralList = New String(12, 1) {{"l", "is"}, {"el", "eis"}, {"ol", "ois"}, {"ul", "uis"}, {"ou", "aram"}, {"oi", "oram"}, {"ão", "ães"}, {"m", "ns"}, {"er", "eres"}, {"r", "res"}, {"z", "zes"}, {"l", "s"}, {"ção", "ções"}}

                singularList = New String(7, 1) {{"il", "eis"}, {"oi", "oram"}, {"ão", "ães"}, {"m", "ns"}, {"er", "eres"}, {"r", "res"}, {"z", "zes"}, {"ção", "ções"}}

                ExceptionsList = New String(13, 1) {{"mal", "males"}, {"férias", "férias"}, {"ônibus", "ônibus"}, {"cônsul", "cônsules"}, {"órfãos", "órfães"}, {"sótãos", "sótães"}, {"órgãos", "órgães"}, {"cidadão", "cidadãos"}, {"cidadãos", "cidadães"}, {"irmãos", "irmães"}, {"cristãos", "cristães"}, {"inútil", "inúteis"}, {"réptil", "répteis"}, {"eu", "eu"}}

                endingsList = (New String() {"ada", "anca", "ancia", "cao", "dao", "enca", "ez", "eza", "ismo", "mento", "sao", "tude", "ura", "al", "alha", "aria", "eria", "agem", "ario", "eiro", "eira", "ia", "ite", "io", "ismo", "ada", "ugem", "dade", "anca", "dor", "douro", "ar", "avel", "oso", "ante", "ano", "udo", "ento", "ao", "ona", "alhao", "arrao", "zarrao", "eirao", "aca", "aco", "orra", "inho", "inha", "zinho", "zinha", "zito", "zita", "ito", "ita", "ete", "eto", "eta", "ote", "ota", "eco", "eca", "ico", "ica", "izar", "ecer", "ear", "in", "des", "re"}).ToList

            ElseIf (lang = "EN") Then
                endingsList = (New String() {"e", "ed", "er", "es", "edly", "ing", "ion", "able", "ally"}).ToList
            Else
                Throw New Exception("Invalid language")
            End If
        End Sub

        Private Function PercentTheSame(ByVal Text As String, ByVal CompareWith As String) As Single
            Dim lonLenText As Long, lonLenCompare As Long
            Dim lonLoop As Long, lonDiff As Long
            Dim strCur As String, strC As String

            lonLenText = Len(Text)
            lonLenCompare = Len(CompareWith)

            For lonLoop = 1 To lonLenText

                If lonLoop > lonLenCompare Then
                    lonDiff = lonDiff + 1
                Else

                    strCur = LCase$(Mid$(Text, lonLoop, 1))
                    strC = LCase$(Mid$(CompareWith, lonLoop, 1))

                    If Not strCur = strC Then
                        lonDiff = lonDiff + 1
                    End If

                End If

            Next lonLoop

            PercentTheSame = CSng(((lonLenText - lonDiff) / lonLenText) * 100)
        End Function

        Private Function StartCompare(ByVal Text As String, ByVal CompareWith As String) As Single

            If Text = CompareWith Then
                StartCompare = 100
                Exit Function
            End If

            If Len(Text) > Len(CompareWith) Then
                StartCompare = PercentTheSame(Text, CompareWith)
            Else
                StartCompare = PercentTheSame(CompareWith, Text)
            End If

        End Function

        Private Function RemoveEspecials(ByVal text As String) As String
            Dim vPos As Byte

            Const vComAcento = "ÀÁÂÃÄÅÇÈÉÊËÌÍÎÏÒÓÔÕÖÙÚÛÜàáâãäåçèéêëìíîïòóôõöùúûü"
            Const vSemAcento = "AAAAAACEEEEIIIIOOOOOUUUUaaaaaaceeeeiiiiooooouuuu"

            For i = 1 To Len(text)
                vPos = InStr(1, vComAcento, Mid(text, i, 1))
                If vPos > 0 Then
                    Mid(text, i, 1) = Mid(vSemAcento, vPos, 1)
                End If
            Next

            Return text
        End Function

#End Region

    End Class

End Namespace

