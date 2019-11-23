window.addEventListener('load', () => {
    const canvas = document.getElementById("canvas");
    const ctx = canvas.getContext("2d");

    const topMargin = 0.25*window.innerHeight;
    var clientX;
    var clientY;

    //Resizing
    canvas.height = window.innerWidth;
    canvas.width = window.innerWidth;
    ctx.lineWidth = 10;      // taille tracé
    ctx.lineCap = "round";   // Forme tracé
    ctx.strokeStyle = "black"; // Couleur

    //variables
    let painting = false;

    function startPosition(e){
        painting = true;
        draw(e);
    }
    function finishedPosition(){
        painting = false;
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