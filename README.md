# VRCDebug

![image](https://github.com/fkrisi11/VRCDebugPlus/assets/26310365/736d6772-4c33-4804-9349-a4cca2545f2c)
An external program that uses OSC to read and write values on your avatar.<br>
Just start it up, give it a moment to connect, and you can start debugging as soon as the parameters appear :)
<br><br>

<h2>Common issues</h2>
The program doesn't want to connect? Check if you have OSC enabled in your radial menu (Options -> OSC) in VRChat.<br>
There are some parameters missing from my avatar...? Reset your OSC config in your radial menu (Options -> OSC) in VRChat.
<br>

<h3>What do the colours mean?</h3>
<b>Yellow (Orange in light theme):</b> This parameter has to update once to show its real value. (This is because of a bug with OSCQuery)<br>
<b>Grayed out value:</b> This is a built-in parameter that can't be changed from the program.
<br>
<h3>Things to know about the UI</h3>
<ul>
  <li>
    Hovering values in the name field will show extra info about the parameter in the box on the left
  </li>
  <li>
    Hovering over the parameter count text will show you how many parameters you have per type
  </li>
  <li>
    You can drag the window around by holding it anywhere, other than the parameter table, and the link labels
  </li>
  <li>
    Middle clicking the window (not the titlebar of it) will reset it back to its default size
  </li>
  <li>
    Clicking any header in the data table will sort the entire table by that column first, and in ABC order second. Clicking it again reverses the sorting order.
  </li>
  <li>
    Unsort the list by right clicking or middle clicking the data table's header
  </li>
  <li>
    Clicking the Avatar ID, above the data table, will take you to the VRChat page of that avatar
  </li>
  <li>
    Clicking the link at the bottom left will take you to the local OSCQuery website where you can see all the avatar values as json data
  </li>
</ul>
<br>
<b>Searching</b><br>
You can find a search bar below the data list, where you can type in your keywords to find your parameters<br>
Just typing in words (separated by commas or spaces) will get you fuzzy search results, searching for partial matches<br>
Using "double" or 'single' quotation marks will search for exact matches. You can mix and match fuzzy and exact match search terms<br>
/ and \ are interchangeable for ease of use

<br>
<h3>What can I enter into the value field?</h3>
<b>Bool:</b> Since they only have 2 values, editing them will flip their values<br>
<b>Integer:</b> These fields take whole numbers<br>
<b>Float:</b> These fields take a number between -1 and 1, separated by a decimal point (or comma) if needed
<br>
<h3>Controls</h3>
Values can be edited by:
<ul>
  <li>
    Double left clicking
  </li>
  <li>
    Right clicking
  </li>
  <li>
    Enter key
  </li>
  <li>
    Space key
  </li>
  <li>
    F2
  </li>
  <li>
    F3
  </li>
</ul>
Note: editing a bool value will change it to its opposite value immediately
<br>
<h1>Download here</h1>

[Latest release](https://github.com/fkrisi11/VRCDebugPlus/releases/latest)
