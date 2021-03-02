Imports System.Text.RegularExpressions

Namespace IA

    Public Class Token
        Private Type As String
        Private value As String
        Private line As Object

        Public Sub New(Type As String, value As String, line As Object)
            Me.Type = Type
            Me.value = value
            Me.line = line
        End Sub

        Public Overrides Function ToString() As String
            Return Me.value
        End Function

    End Class

    Public Class NLP

        Public Enum Language
            PT
            EN
        End Enum

        Public Enum Transform
            LowerCase
            SentenceCase
            TitleCase
            UpperCase
        End Enum

        Private endingsList As List(Of String)
        Private pluralList As String(,)
        Private ReadOnly Extras As String(,)
        Private ExceptionsList As String(,)
        Private singularList As String(,)
        Private Max As Integer = 0
        Private pluraliseCount As Integer = 0
        Private Max3 As Integer = 0

        Public Sub New(lang As Language)
            PopulateList(lang)
        End Sub

        Public Function Stem(ByVal stringText As String) As String

            Dim result As String = RemoveEspecials(stringText).ToLower()

            For index As Integer = 0 To endingsList.Count - 1
                If result.EndsWith(endingsList(index)) Then
                    result = stringText.Substring(0, stringText.Length - endingsList(index).Length)
                    Exit For
                End If
            Next

            Return result

        End Function

        Public Function WordTokenize(ByVal stringText As String) As String()
            If (stringText.Contains(" ")) Then
                Return Regex.Split(stringText, " ")
            Else
                Return New String() {stringText}
            End If
        End Function

        Public Function WordsLike(ByVal word1 As String, word2 As String) As Single
            Return StartCompare(word1, word2)
        End Function

        Public Function Singularize(ByVal stringText As String) As String
            Try
                Dim result As String = stringText
                Dim textprocess = stringText.Split(" ")

                For txt As Integer = 0 To textprocess.Length - 1
                    If (textprocess(txt).Trim.Length > 0) Then
                        Dim skip As Boolean = False

                        For index As Integer = 0 To pluraliseCount
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
                        For index As Integer = 0 To pluraliseCount
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

        Public Function Levenshtein(ByVal texte1 As String, ByVal text2 As String, Optional ByVal tolower As Boolean = False) As Integer
            If (tolower) Then
                texte1 = texte1.ToLower()
                text2 = text2.ToLower()
            End If
            Dim input1 As Integer = texte1.Length
            Dim input2 As Integer = text2.Length
            Dim array2d(input1 + 1, input2 + 1) As Integer

            If input1 = 0 Then
                Return input2
            End If
            If input2 = 0 Then
                Return input1
            End If

            Dim i As Integer
            Dim j As Integer

            For i = 0 To input1
                array2d(i, 0) = i
            Next
            For j = 0 To input2
                array2d(0, j) = j
            Next
            For i = 1 To input1
                For j = 1 To input2
                    Dim cost As Integer
                    If text2(j - 1) = texte1(i - 1) Then
                        cost = 0
                    Else
                        cost = 1
                    End If
                    array2d(i, j) = Math.Min(Math.Min(array2d(i - 1, j) + 1, array2d(i, j - 1) + 1), array2d(i - 1, j - 1) + cost)
                Next
            Next
            Return array2d(input1, input2)
        End Function

        Public Function Pluralize(ByVal stringText As String) As String
            Try
                Dim result As String = stringText
                Dim textprocess = stringText.Split(" ")

                For txt As Integer = 0 To textprocess.Length - 1
                    If (textprocess(txt).Trim.Length > 0) Then
                        Dim skip As Boolean = False

                        For index As Integer = 0 To pluraliseCount
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
                        For index As Integer = 0 To pluraliseCount
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

        Public Function Tokenize(ByVal input As String)
            Dim inputArray As String() = input.Split(" ")
            Dim result As List(Of Token) = New List(Of Token)
            For i As Integer = 0 To inputArray.Count - 1
                Dim word As String = inputArray(i)
                If (word IsNot "") Then
                    Dim notMarkAsSpecial As Boolean = True
                    Dim notMarkAsIntenger As Boolean = True
                    Dim notMarkAsOperator As Boolean = True

                    Dim outPutInnerWords As List(Of String) = New List(Of String)
                    Dim outPutInnerWordsDigit As List(Of String) = New List(Of String)
                    Dim outPutInnerWordsOperator As List(Of String) = New List(Of String)

                    For j As Integer = 0 To word.Count - 1
                        If (isEspecialCharacter(word(j))) Then
                            outPutInnerWords.Add(word(j))
                        End If
                    Next

                    For j As Integer = 0 To word.Count - 1
                        If (isDigit(word(j))) Then
                            outPutInnerWordsDigit.Add(word(j))
                        End If
                    Next

                    For j As Integer = 0 To word.Count - 1
                        If (isOperator(word(j))) Then
                            outPutInnerWordsOperator.Add(word(j))
                        End If
                    Next

                    If (isDigit(word)) Then
                        notMarkAsIntenger = False
                        result.Add(New Token("Literal", word, i))
                    ElseIf (isLetter(word)) Then
                        result.Add(New Token("Text", word, i))
                    ElseIf (isOperator(word)) Then
                        notMarkAsOperator = False
                        result.Add(New Token("Operator", word, i))
                    ElseIf (isEspecialCharacter(word)) Then
                        notMarkAsSpecial = False
                        result.Add(New Token("Especial character", word, i))
                    Else
                        result.Add(New Token("Unknow", word, i))
                    End If

                    If (notMarkAsSpecial) Then
                        For j As Integer = 0 To outPutInnerWords.Count - 1
                            result.Add(New Token("Especial character", outPutInnerWords(j), i))
                        Next
                    End If

                    If (notMarkAsIntenger) Then
                        For j As Integer = 0 To outPutInnerWordsDigit.Count - 1
                            result.Add(New Token("Literal", outPutInnerWordsDigit(j), i))
                        Next
                    End If

                    If (notMarkAsOperator) Then
                        For j As Integer = 0 To outPutInnerWordsOperator.Count - 1
                            result.Add(New Token("Operator", outPutInnerWordsOperator(j), i))
                        Next
                    End If

                End If
            Next
            Return result
        End Function


        Public Shared Function TransformText(ByVal stringText As String, ByVal modifier As Transform) As String
            Select Case modifier
                Case Transform.LowerCase
                    Return StrConv(stringText, VbStrConv.Lowercase)
                Case Transform.SentenceCase
                    Dim input = stringText.Split(" ")
                    Dim OutPut As String = ""
                    For i As Integer = 0 To input.Length - 1
                        If (i = 0) Then
                            OutPut += StrConv(input(i), VbStrConv.ProperCase)
                        Else
                            OutPut += input(i)
                        End If
                        If (input.Length > 1) Then
                            OutPut += " "
                        End If
                    Next
                    Return OutPut
                Case Transform.TitleCase
                    Return StrConv(stringText, VbStrConv.ProperCase)
                Case Transform.UpperCase
                    Return StrConv(stringText, VbStrConv.Uppercase)
                Case Else
                    Throw New Exception("Invalid Transform")
            End Select
        End Function

        Public Shared Function RemoveEspecials(ByVal text As String) As String
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

#Region "Privates"

        Private Function isComma(ByVal input As String)
            Return input = ","
        End Function

        Private Function isDigit(ByVal input As String)
            Return New Regex("^[0-9 ]+$").IsMatch(input)
        End Function

        Private Function isLetter(ByVal input As String)
            Return New Regex("^[a-z]", RegexOptions.IgnoreCase).IsMatch(input)
        End Function

        Private Function isOperator(ByVal input As String)
            Return New Regex("/\+|-|\*|\/|\^/").IsMatch(input)
        End Function

        Private Function isEspecialCharacter(ByVal input As String)
            Return New Regex("[~`!@#$%^&*()-+=|{}':;.,<>/?]").IsMatch(input)
        End Function

        Private Sub PopulateList(ByVal lang As Language)
            If (lang.Equals(Language.PT)) Then
                pluralList = New String(12, 1) {{"l", "is"}, {"el", "eis"}, {"ol", "ois"}, {"ul", "uis"}, {"ou", "aram"}, {"oi", "oram"}, {"ão", "ães"}, {"m", "ns"}, {"er", "eres"}, {"r", "res"}, {"z", "zes"}, {"l", "s"}, {"ção", "ções"}}
                singularList = New String(7, 1) {{"il", "eis"}, {"oi", "oram"}, {"ão", "ães"}, {"m", "ns"}, {"er", "eres"}, {"r", "res"}, {"z", "zes"}, {"ção", "ções"}}
                ExceptionsList = New String(13, 1) {{"mal", "males"}, {"férias", "férias"}, {"ônibus", "ônibus"}, {"cônsul", "cônsules"}, {"órfãos", "órfães"}, {"sótãos", "sótães"}, {"órgãos", "órgães"}, {"cidadão", "cidadãos"}, {"cidadãos", "cidadães"}, {"irmãos", "irmães"}, {"cristãos", "cristães"}, {"inútil", "inúteis"}, {"réptil", "répteis"}, {"eu", "eu"}}
                endingsList = (New String() {"ada", "anca", "ancia", "cao", "dao", "enca", "ez", "eza", "ismo", "mento", "sao", "tude", "ura", "al", "alha", "aria", "eria", "agem", "ario", "eiro", "eira", "ia", "ite", "io", "ismo", "ada", "ugem", "dade", "anca", "dor", "douro", "ar", "avel", "oso", "ante", "ano", "udo", "ento", "ao", "ona", "alhao", "arrao", "zarrao", "eirao", "aca", "aco", "orra", "inho", "inha", "zinho", "zinha", "zito", "zita", "ito", "ita", "ete", "eto", "eta", "ote", "ota", "eco", "eca", "ico", "ica", "izar", "ecer", "ear", "in", "des", "re"}).ToList
            ElseIf (lang.Equals(Language.EN)) Then
                endingsList = (New String() {"e", "ed", "er", "es", "edly", "ing", "ion", "able", "ally"}).ToList
                pluralList = New String(4, 1) {{"s", "es"}, {"x", "es"}, {"ch", "es"}, {"sh", "es"}, {"y", "ies"}}
                singularList = New String(3, 1) {{"es", "s"}, {"es", "x"}, {"es", "ch"}, {"es", "sh"}}
            Else
                Throw New Exception("Invalid language")
            End If
            pluraliseCount = (pluralList.Length / 2)
            Max = pluraliseCount - 1
            Max3 = (singularList.Length / 2) - 1
        End Sub

        Private Function PercentTheSame(ByVal Text As String, ByVal CompareWith As String) As Single
            Dim lonLenText As Long, lonLenCompare As Long
            Dim lonLoop As Long, lonDiff As Long
            Dim strCur As String, strC As String

            lonLenText = Len(Text)
            lonLenCompare = Len(CompareWith)

            For lonLoop = 1 To lonLenText

                If lonLoop > lonLenCompare Then
                    lonDiff += 1
                Else

                    strCur = LCase$(Mid$(Text, lonLoop, 1))
                    strC = LCase$(Mid$(CompareWith, lonLoop, 1))

                    If Not strCur = strC Then
                        lonDiff += 1
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
#End Region

    End Class

End Namespace

