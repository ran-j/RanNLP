# RanNLP

A simple NLP for .Net 

The following functions are available:

| Function            | Language    | Explanation                                                             |
|:-----------------------|:------------|:------------------------------------------------------------------------|
| Singularize            | Portugese         |  In development, not working perfectly |
| Pluralize     | Portugese         |   In development, not working perfectly |
| WordTokenize     | All         |   None |
| WordsLike     | All         |   None |
| TransformText     | all      |   None | 
| RemoveEspecials     | all      |   None | 
| Levenshtein     | all      |   None | 
| Stem     | English ,Portugese       |   None |


Examples

# Singularize
Turn words into singular (Very Unstable)

```vb
Private RanNLP As New NLP("PT")
MsgBox(NLP.Singularize("abordagens"))
 'Return "abordagem"
```

# Pluralize
Turn words into plural

```vb
Private RanNLP As New NLP("PT")
MsgBox(NLP.Pluralize("casa"))
 'Return "casas"
```
# Words Like
Check similarity of words and return a float with confidentiality

```vb
Private RanNLP As New NLP("PT")
MsgBox(NLP.WordsLike("casa","casarão"))
 'Return 57.14286
```

# Stem

Stem words
```vb
Private RanNLP As New NLP("PT")
MsgBox(NLP.Stem("papelaria"))
 'Return "papel"
```

# TransformText

TransformText text
```vb
Debug.Print(NLP.TransformText("Ola mundo", Transform.LowerCase))
Debug.Print(NLP.TransformText("Ola mundo", Transform.SentenceCase))
Debug.Print(NLP.TransformText("Ola mundo", Transform.TitleCase))
Debug.Print(NLP.TransformText("Ola mundo", Transform.UpperCase))
 'Returns 
    'ola mundo'
    'Ola mundo'
    'Ola Mundo'
    'OLA MUNDO'
```

# RemoveEspecials

```vb
Debug.Print(NLP.RemoveEspecials("eleição"))
 'Return eleicao
```

# Levenshtein

```vb
Private RanNLP As New NLP("PT")

Dim LowerAllText As Boolean = True

Debug.Print(RanNLP.Levenshtein("casa", "Casarão"))
Debug.Print(RanNLP.Levenshtein("casa", "Casarão", LowerAllText))
'Returns 
    '4'
    '3'
```
