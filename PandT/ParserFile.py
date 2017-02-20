#import jpype
import nltk

# TODO: Chunking and Chinking using nltk | Dep Parsing using spacy |

# Initialize lemmatizer
lemmatizer = nltk.WordNetLemmatizer()

common_phrases = [("DOING WELL", "good")]

variables = [[],[],[]]
#tagged_sentences = []
#signs = []
#sequence = []

pos_functions = locals()

# def hash_pos(pos):
#     hashed = 0
#     length = len(pos)
#     for i in range(length):
#         hashed += pow(10, length-i-1)*ord(pos[i])
#     return hashed

# Function pointers
# pos_functions = [None]*(hash_pos('WRB'))

# Define functions for each part of speech
def P(i, x, word, pos):
    if word == '?':
        variables[1][i][0] = 'QUESTION'
        variables[1][i].append("Q")
    elif word == '!':
        variables[1][i][0] = 'EXCLAMATION'
def CC(i, x, word, pos):
    temp = 0
def CD(i, x, word, pos):
    temp = 0
def DT(i, x, word, pos):
    if word.upper() not in ["THE"]:
        variables[1][i].append(word.upper())
def EX(i, x, word, pos):
    temp = 0
def FW(i, x, word, pos):
    temp = 0
def IN(i, x, word, pos):
    # Handle the ambiguous word 'THOUGH'
    if word.upper() == 'THOUGH' or word.upper() == 'ALTHOUGH':
        # If 'THOUGH' is the first word of the sentence
        if x == 0:
            variables[1][i].append('NO-MATTER')
        else:
            variables[1][i].insert(1, 'BUT')
def JJ(i, x, word, pos):
    if not x - 1 < 0:
        prev = variables[0][i][x - 1][0].upper()
        if prev == 'MORE':
            variables[1][i].append(word.upper() + '-R')
        elif prev == 'MOST':
            variables[1][i].append(word.upper() + '-S')
        else:
            variables[1][i].append(word.upper())
def JJR(i, x, word, pos):
    variables[1][i].append(lemmatizer.lemmatize(word, pos="a").upper() + '-R')
def JJS(i, x, word, pos):
    variables[1][i].append(lemmatizer.lemmatize(word, pos="a").upper() + '-S')
def LS(i, x, word, pos):
    temp = 0
def MD(i, x, word, pos):
    temp = 0
def NN(i, x, word, pos):
    variables[1][i].append(word.upper())
def NNS(i, x, word, pos):
    variables[1][i].append(lemmatizer.lemmatize(word, pos='n').upper())
def NNP(i, x, word, pos):
    variables[1][i].append(word.upper())
def NNPS(i, x, word, pos):
    temp = 0
def PDT(i, x, word, pos):
    temp = 0
def POS(i, x, word, pos):
    temp = 0
def PRP(i, x, word, pos):
    # Check for 'I'
    if word.upper() == 'ME' or word.upper() == 'I':
        variables[1][i].append('ME')
    # Check for 'he', 'she' and 'it'
    elif word.upper() == 'HE' or word.upper() == 'SHE' or word.upper() == 'IT':
        variables[1][i].append('IT')
    # Check for 'you'
    elif word.upper() == 'YOU':
        variables[1][i].append('YOU')
def PRPP(i, x, word, pos):
    variables[1][i].append(lemmatizer.lemmatize(word).upper())
def RB(i, x, word, pos):
    # Handle the ambiguous word 'WELL'
    if word.upper() == 'WELL':
        # If 'WELL' is not the first word of the sentence
        if x != 0:
            variables[1][i].append("GOOD")
def RBR(i, x, word, pos):
    temp = 0
def RBS(i, x, word, pos):
    temp = 0
def RP(i, x, word, pos):
    temp = 0
def SYM(i, x, word, pos):
    temp = 0
def TO(i, x, word, pos):
    temp = 0
def UH(i, x, word, pos):
    temp = 0
def VB(i, x, word, pos):
    if (not x - 1 < 0) and (
        variables[0][i][x - 1][0].upper() == 'N\'T' or variables[0][i][x - 1][0].upper == 'NOT'):
        variables[1][i].append("NOT-" + word.upper())
    else:
        variables[1][i].append(word.upper())
def VBD(i, x, word, pos):
    variables[1][i].append(lemmatizer.lemmatize(word, pos='v').upper())
def VBG(i, x, word, pos):
    temp = 0
def VBN(i, x, word, pos):
    temp = 0
def VBP(i, x, word, pos):
    if not (word.upper() == '\'M' or lemmatizer.lemmatize(word.lower(),pos='v').upper() == 'BE' or lemmatizer.lemmatize(word.lower(),pos='v').upper() == "DO"):
        variables[1][i].append(word.upper())
def VBZ(i, x, word, pos):
    temp = 0
def WDT(i, x, word, pos):
    temp = 0
def WP(i, x, word, pos):
    temp = 0
def WPP(i, x, word, pos):
    temp = 0
def WRB(i, x, word, pos):
    variables[1][i].append(word.upper())

def pre_processing():
    for tuple in common_phrases:
        if tuple[0] in inputSent.upper():
            print("TODO: Pre-processing")


# Results in an array of sentences that are tagged by their
def tag_sentences(text):
    variables[0] = nltk.sent_tokenize(text)
    for i in range(len(variables[0])):
        variables[0][i] = nltk.word_tokenize(variables[0][i], 'english')
        variables[0][i] = nltk.pos_tag(variables[0][i])


# Takes an array of tokenized and tagged sentences.
# Returns array of signs for all of the sentences
# LIST OF TAGS AT: https://catalog.ldc.upenn.edu/docs/LDC99T42/tagguid1.pdf
def to_signs():
    for i in range(len(variables[0])):
        # By default, the sentence has a neutral emotion
        variables[1].append(['NEUTRAL'])
        # Iterate through tags in this sentence
        for x in range(len(variables[0][i])):
            word = variables[0][i][x][0]
            pos = variables[0][i][x][1]
            if pos == '.' or pos == ',':
                P(i, x, word, pos)
            elif pos == 'PRP$':
                PRPP(i, x, word, pos)
            elif pos == 'WP$':
                WPP(i, x, word, pos)
            else:
                pos_functions[pos](i, x, word, pos)
        variables[1][i].append(".")


def generate_sequence():
    for sentence in variables[1]:
        for sign in sentence:
            variables[2].append(sign)


def text_to_sign(input_text):
    pre_processing()
    tag_sentences(input_text)
    to_signs()
    generate_sequence()
    return variables[2]

inputSent = "How are you doing? My name is Onur. Well, I don't know. I'm doing well. Though he is smart, he got all of the questions wrong. The weather is not great though! Don't say that. This is the most comfortable place."


#print(variables[0])
#print(variables[1])
#print(variables[2])

# print(processInput(inputSent)[0][0][1] + ": WRB or WH-ABVERB (where, when etc.)")
# print(processInput(inputSent)[0][1][1] + ": VBP or VERB PRESENT (take)")
# print(processInput(inputSent)[0][2][1] + ": PRP or PERSONAL PRONOUN (I, he, it)")
# print(processInput(inputSent)[0][3][1] + ": End-Punctuation, problem area")
# TODO http://www.winwaed.com/blog/2011/11/08/part-of-speech-tags/
# def startJvm():
#    import os
#    os.environ.setdefault("STANFORD_PARSER_HOME", "3rdParty/stanford-parser/stanford-parser-2010-08-20")
#    global stanford_parser_home
#    stanford_parser_home = os.environ["STANFORD_PARSER_HOME"]
#    jpype.startJVM(jpype.getDefaultJVMPath(),
#                  "-ea",
#                   "-Djava.class.path=%s/stanford-parser.jar" % (stanford_parser_home),)
#startJvm() # one jvm per python instance.

# from parser import PandT
# stanford_parser = PandT()

# dependencies = stanford_parser.parseToStanfordDependencies("The girl I met was your sister.")
#The/DT girl/NN I/PRP met/VBD was/VBD your/PRP$ sister/NN ./.
# tupleResult = [(rel, gov.text, dep.text) for rel, gov, dep in dependencies.dependencies]
# print(tupleResult)