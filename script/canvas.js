window.addEventListener('load', () => {
    const canvas = document.getElementById("canvas");
    const ctx = canvas.getContext("2d");
    
    const topMargin = 0.25*window.innerHeight;
    var clientX;
    var clientY;
    var penColor = "black"
    var backgroundColor = "white";

    var buttonPen = document.getElementById("buttonPen");
    var buttonColor = document.getElementById("buttonColor");
    var buttonRubber = document.getElementById("buttonRubber");
    var buttonSize = document.getElementById("buttonSize");
    var buttonReturn = document.getElementById("buttonReturn");
    var buttonPaint = document.getElementById("buttonPaint");
    var sizeRange = document.getElementById("sizeRange");
    var outputRange = document.getElementById("displayDrawSize");
    

    //Resizing
    canvas.height = window.innerWidth;
    canvas.width = window.innerWidth;
    ctx.lineWidth = 10;      // taille tracé
    ctx.lineCap = "round";   // Forme tracé
    ctx.strokeStyle = penColor; // Couleur

    outputRange.style.width = ctx.lineWidth; //Affiche la valeur par défaut du slider
    outputRange.style.height = ctx.lineWidth;

    //variables
    let painting = true;

    function startPosition(e){
        //painting = true;
        draw(e);
    }
    function finishedPosition(){
        //painting = false;
        ctx.beginPath();
    }
    function draw(e){
        clientX = e.touches[0].clientX;
        clientY = e.touches[0].clientY - topMargin;
        if(!painting) return;

        ctx.lineTo(clientX, clientY);
        ctx.stroke();
        ctx.beginPath();
        ctx.moveTo(clientX, clientY);
    }

    /************** BOUTONS **************/
    buttonPen.onclick = function() {
        sizeRange.style.display="none";
        ctx.strokeStyle = penColor;
    }
    buttonColor.onclick = function() {
        sizeRange.style.display="none";
    }
    buttonRubber.onclick = function() {
        sizeRange.style.display="none";
        ctx.strokeStyle = backgroundColor;
    }
    /*** Slider taille ***/
    buttonSize.onclick = function() {
        sizeRange.style.display="initial";
    }

    buttonReturn.onclick = function() {
        sizeRange.style.display="none";
    }
    buttonPaint.onclick = function() {
        sizeRange.style.display="none";
        if(backgroundColor == "red")
        backgroundColor = "white";
        else
        backgroundColor = "red";
        canvas.style.background = backgroundColor;
    }

    sizeRange.oninput = function() {
        ctx.lineWidth = this.value;
        outputRange.style.width = ctx.lineWidth+"px"; //Affiche la valeur par défaut du slider
        outputRange.style.height = ctx.lineWidth+"px";
    }
    //canvas.style.background = "red";
    
    //EventListenners
    /* POUR NAVIGATEUR WEB
     * canvas.addEventListener('mousedown',startPosition);
     * canvas.addEventListener('mouseup',finishedPosition);
     * canvas.addEventListener('mousemove',draw);
     */
    /* POUR NAVIGATEUR MOBILE 
     */
    canvas.addEventListener('touchstart',startPosition);
    canvas.addEventListener('touchend',finishedPosition);
    canvas.addEventListener('touchmove',draw);
});