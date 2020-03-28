// Arrays to store user inputs
var subjectId
var recall = []
var subjectWrapper = []
var subject = {}
var recallOrder = []
var visualOnShow = false

// Counter variables for iteration, default 0
var distractionCounter = 0
var recallCounter = 0

// Timeout values
var digitCount = 7 //Digits shown in a row (each one second) - default 7
var distractions = 9 //Amount of distraction tasks to be conducted by subject - default 9
var recalls = 37 //Amount of value pairs asked in the recall section - default 37


// Hide Elements in the DOM
function Hide(id) {
    document.getElementById(id).style.display = "none"
}

// Show Elements in the DOM
function Show(id) {
    document.getElementById(id).style.display = "flex"
}


function SetAdmin() {
    
    // Get subject Id and store it
    subjectId = document.getElementById("subjectId").value
    subject.Id = subjectId

    // Create new list of required recall items
    for (i=0;i < recalls+1; i++) {
        recallOrder.push(i);
    }

    // Shuffle recall order
    shuffle(recallOrder)

    // Update DOM
    Hide("Admin")
    Show("Start")
}

// Shuffle an array
function shuffle(array) {
    var currentIndex = array.length, temporaryValue, randomIndex
  
    // While there remain elements to shuffle...
    while (0 !== currentIndex) {
  
      // Pick a remaining element...
      randomIndex = Math.floor(Math.random() * currentIndex)
      currentIndex -= 1
  
      // And swap it with the current element.
      temporaryValue = array[currentIndex]
      array[currentIndex] = array[randomIndex]
      array[randomIndex] = temporaryValue
    }
    return array
  }


function StartQuestionnaire() {
    
    // Get subject details
    subject.age = document.getElementById("subjectAge").value
    subject.nationality = document.getElementById("subjectNationality").value
    subject.sex = document.getElementById("subjectSex").value

    // Update DOM
    Hide("Start")
    Show("Start-Distraction")
}


function StartDistraction() {

    // Update DOM
    Hide("Start-Distraction");
    Show("Distraction-Task");

    // Get first distraction
    RunDistraction();
}


function RunDistraction() {

    // Check if new distraction is wanted
    if(distractionCounter <= distractions) {

        // Hide controls
        Hide("Distraction-Form");

        // Show new numbers
        var timeleft = digitCount;
            var downloadTimer = setInterval(function(){
                var prevNumber = document.getElementById("Digit").innerHTML
                var newNumber = Math.floor((Math.random() * 9)+1);
                //Avoid showing the same number twice
                if (prevNumber == newNumber) {
                    document.getElementById("Digit").innerHTML = Math.floor((Math.random() * 9)+1);
                }
                else {
                    document.getElementById("Digit").innerHTML = newNumber;
                }
                //End updating numbers when time expired
                if(timeleft <= 0){
                    clearInterval(downloadTimer);
                    document.getElementById("Digit").innerHTML = "&nbsp;";
                    Show("Distraction-Form");
                    document.getElementById("Distraction-Input").value = "";
                    document.getElementById("Distraction-Input").focus();
                }
                timeleft -= 1;
            }, 1000);
    }

    // If on new distraction is wanted
    else {

        // Update DOM
        Hide("Distraction-Task");
        Show("Start-Recall");

    }

    // Update counter after each iteration
    distractionCounter++;
}


function NextDistraction() {    

    // Update DOM
    Hide("Distraction-Form");

    // get new distraction
    RunDistraction();
}

function StartRecall() {

    // Update DOM
    Hide("Start-Recall");
    Show("Recall-Task");

    // Show first recall
    RunRecall();
}


// Create a new recall
function RunRecall() {

    // Check if recall is needed
    if (recallCounter <= recalls) {

        // Update headline in DOM
        document.getElementById("PairCount").innerHTML = "Pair " + String(recallCounter + 1) + " of " + String(recalls + 1);
        
        // Show visual or audio randomly
        var stimulus = document.getElementById('Stimulus');
        var visual = '<img id="Visual" src=""></img>'
        var audio = '<figure><figcaption>Click to play the sound.</figcaption><audio id="Audio" controls src="">Your browser does not support the<code>audio</code>element.</audio></figure>'
        if (Math.random() >= 0.5) {
            stimulus.innerHTML = visual;
            document.getElementById("Visual").src = "../Assets/Resources/Sprites/pair" + String(recallOrder[recallCounter]) + "_visual.jpg"
            visualOnShow = true;
        }
        else {
            stimulus.innerHTML = audio;
            document.getElementById("Audio").src = "../Assets/Resources/Sounds/pair" + String(recallOrder[recallCounter]) + "_audio.mp3"
            visualOnShow = false;
        }

        // Reset inputs
        document.getElementById("confidenceEntry").value = .5;
        document.getElementById("stimulusEntry").value = "";
    }

    // Exit if no new recall needed
    else {
        Hide("Recall-Task");
        Show("End");
    }
}


function SubmitRecall() {
    // Create and store an entry for each submission
    entry = [recallOrder[recallCounter], [document.getElementById("stimulusEntry").value, document.getElementById("confidenceEntry").value, visualOnShow]]
    recall.push(entry)
    
    // Update counter and get new recall
    recallCounter++
    RunRecall();
}


function EndAdmin() {
    // Update DOM
    Hide("End");
    Show("Download");

    // Sort entries and push final object into array to be printed
    CleanResults(recall);
    subjectWrapper.push(subject)
}


function CleanResults(array) {
    // iterate through all recall ids
    for (i=0;i < recalls+1; i++) {

        // iterate through all entries
        for (j=0; j < array.length; j++) {
            // find the current recall id
            if (array[j][0] == i) {
                // add data to subject object
                subject["entry_" + i] = array[j][1][0]
                subject["confidence_" + i] = array[j][1][1]
                subject["visualStimulus_" + i] = array[j][1][2]
            }
        }
    }
}

// Convert Object to CSV
function ConvertArrayOfObjectsToCSV(args) {
    
    var result, ctr, keys, columnDelimiter, lineDelimiter, data;

    data = args.data || null;
    if (data == null || !data.length) {
        return null;
    }

    columnDelimiter = args.columnDelimiter || ',';
    lineDelimiter = args.lineDelimiter || '\n';

    keys = Object.keys(data[0]);

    result = '';
    result += keys.join(columnDelimiter);
    result += lineDelimiter;

    data.forEach(function(item) {
        ctr = 0;
        keys.forEach(function(key) {
            if (ctr > 0) result += columnDelimiter;

            result += item[key];
            ctr++;
        });
        result += lineDelimiter;
    });

    return result;
}


// Download CSV for Word Ratings
function DownloadLog(args) {
    var data, filename, link;
    var csv = ConvertArrayOfObjectsToCSV({
        data: subjectWrapper
    });
    if (csv == null) return;
    filename = "subject" + subjectId + ".csv" || 'export.csv';
    if (!csv.match(/^data:text\/csv/i)) {
        csv = 'data:text/csv;charset=utf-8,' + csv;
    }
    data = encodeURI(csv);
    link = document.createElement('a');
    link.setAttribute('href', data);
    link.setAttribute('download', filename);
    link.click();
}