Open the graph by checking: "Windows/DS/Graphview"
in the menu on the top of the screen

You can add nodes by pressing space or right click and then choosing the correct node.

Press save to save your graph and make sure to give it a special name


If you want to use the dialogue in the graph add the "DSDialogue" component to any gameobject and select your starting dialogue. Then call "StartDialogue()" to start

You should setup a dialoguemanager as in the examplescene

Checkout the exampledialogue in the example scene for all options





Syntax for effects:
No effect: <none>, </>, <n>, </{ANY TEXT}>

//Moves characters op and down in a wave patern
Wave effect: <wavy>, <wave>, <w>

//Shakes the charachters randomly
Shake effect: <shaky>, <shake>, <s>

//Wait for the amount of miliseconds specified
Wait effect: <wait 1000>, <wait=1000> 
(waits for one second)

The default save location is currently:
"Assets/Engine/DialogueSystem/Dialogues"