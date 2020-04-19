This project gets data from:
    -GoogleSheet file (by api googlesheet api credentials)
    -csv file downloaded in the project

You can choose which in which way you are goint to get the data,
updating de data of the 'OptionMode' property, which is located in
the 'config.json' file.

In the config.json file you can:

[In the csv scope]
- define the path of the the csv file that has been located in the project


[In the GoogleSheet scope]
- define the shared path the file of the googlesheet api
- select the name of the spreadsheet
- define de value of A0 (which will be avoided in the parsing process)

Also you can choose one of the two options of the data's retrieving process
1.- GoogleSheet mode.
2.- CSV mode.

