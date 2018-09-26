// For an introduction to the Blank template, see the following documentation:
// http://go.microsoft.com/fwlink/?LinkID=397704
// To debug code on page load in cordova-simulate or on Android devices/emulators: launch your app, set breakpoints, 
// and then run "window.location.reload()" in the JavaScript Console.
(function () {
    // "use strict";

    document.addEventListener( 'deviceready', onDeviceReady.bind( this ), false );

    function onDeviceReady() {
        // Handle the Cordova pause and resume events
        document.addEventListener( 'pause', onPause.bind( this ), false );
        document.addEventListener( 'resume', onResume.bind( this ), false );
        
        // TODO: Cordova has been loaded. Perform any initialization that requires Cordova here.
        var parentElement = document.getElementById('deviceready');
        var listeningElement = parentElement.querySelector('.listening');
        var receivedElement = parentElement.querySelector('.received');
        listeningElement.setAttribute('style', 'display:none;');
        receivedElement.setAttribute('style', 'display:block;');
    };



    function onPause() {
        // TODO: This application has been suspended. Save application state here.
    };

    function onResume() {
        // TODO: This application has been reactivated. Restore application state here.
    };
} )();

    function login() {
       var username = document.getElementById("username").value;
       var passwordEntry = document.getElementById("password").value;

       var data = { username: username , password : passwordEntry };
       $.ajax({  
           type: 'POST',  
           url: 'http://localhost:1234/capstone/ajax/login.php', 
           data: data,
           success: function(response) {
               var pulledData = JSON.parse(response);
               
               if (pulledData != "null") {
                    if(pulledData.type == "1"){
                      window.location.href = 'doctorMain.html'
                    } else {
                      window.location.href = 'patientMain.html'
                    }
               }               
           }
       });
    }
