function toggleBLOCK(ID){
    var cible = document.getElementById(ID);    
    cible.style.display = "block";
}

function toggleNONE(ID){
    var cible = document.getElementById(ID);    
    cible.style.display = "none";
}

/* -----      ----- */
/* -----  ON  ----- */
/* -----      ----- */

function toggleON_Menu(ID){
    var cible = document.getElementById(ID);    
    cible.style.left = "0vw";      
}

function toggleON_Devine(ID){
    var cible = document.getElementById(ID);    
    cible.style.top = "0vw";      
}

/* -----       ----- */
/* -----  OFF  ----- */
/* -----       ----- */

function toggleOFF_Menu(ID){
    var cible = document.getElementById(ID);    
    cible.style.left = "-100vw";      
}

function toggleOFF_Devine(ID){
    var cible = document.getElementById(ID);    
    cible.style.top = "-100vh";      
}


