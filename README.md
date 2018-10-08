# RanNLP

A simple NLP for .Net 

The following functions are available:

| Function            | Language    | Explanation                                                             |
|:-----------------------|:------------|:------------------------------------------------------------------------|
| Singularize            | Portugese         |  In development, not working perfectly |
| Pluralize     | Portugese         |   In development, not working perfectly |
| WordTokenize     | All         |   None |
| WordsLike     | All         |   None |
| Stem     | English ,Portugese       |   None |


Examples

# Singularize
Turn words into singular (Very instable)

```vb
Private NLP As New NLP("PT")
MsgBox(NLP.Pluralize("abordagens"))
 'Return "abordagem"
```

# Pluralize
Turn words into plural

```vb
Private NLP As New NLP("PT")
MsgBox(NLP.Pluralize("casa"))
 'Return "casas"
```
# Words Like
Check similarity of words and return a float with confidentiality

```vb
Private NLP As New NLP("PT")
MsgBox(NLP.WordsLike("casa","casarão"))
 'Return 57.14286
```

# Stem

Stem words
```vb
Private NLP As New NLP("PT")
MsgBox(NLP.Stem("papelaria"))
 'Return "papel"
```
