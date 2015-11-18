// Create the canvas
var canvas = document.createElement("canvas");
var ctx = canvas.getContext("2d");
canvas.width = 512;
canvas.height = 480;
document.body.appendChild(canvas);

// Background image
var bgReady = false;
var bgImage = new Image();
bgImage.onload = function () {
    bgReady = true;
};
bgImage.src = "/Content/background.png";

// Hero image
var heroReady = false;
var heroImage = new Image();
heroImage.onload = function () {
    heroReady = true;
};
heroImage.src = "/Content/hero.png";

// Monster image
var monsterReady = false;
var monsterImage = new Image();
monsterImage.onload = function () {
    monsterReady = true;
};
monsterImage.src = "/Content/monster.png";

// Game objects
var hero = {
    speed: 128,
    x: 0,
    y: 0
};
var monster = {
    speed: 128,
    x: canvas.width - 64,
    y: canvas.height - 64
};
var monstersCaught = 0;

// Handle keyboard controls
var keysDown = {};

addEventListener("keydown", function (e) {
    keysDown[e.keyCode] = true;
}, false);

addEventListener("keyup", function (e) {
    delete keysDown[e.keyCode];
}, false);

// Reset the game when the player catches a monster
var reset = function () {
    if (mycharacter == null)
        return;

    if (mycharacter != null && mycharacter == monster) {
        monster.x = 32 + (Math.random() * (canvas.width - 64));
        monster.y = 32 + (Math.random() * (canvas.height - 64));
    }

    //if (mycharacter != null)
    //    hero.x = canvas.width / 2;
    //hero.y = canvas.height / 2;

    if (mycharacter != null)
        moved();

    //// Throw the monster somewhere on the screen randomly
    //monster.x = 32 + (Math.random() * (canvas.width - 64));
    //monster.y = 32 + (Math.random() * (canvas.height - 64));
};

// Update game objects
var update = function (character, modifier) {
    if (38 in keysDown) { // Player holding up
        character.y -= character.speed * modifier;
    }
    if (40 in keysDown) { // Player holding down
        character.y += character.speed * modifier;
    }
    if (37 in keysDown) { // Player holding left
        character.x -= character.speed * modifier;
    }
    if (39 in keysDown) { // Player holding right
        character.x += character.speed * modifier;
    }

    if (38 in keysDown || 40 in keysDown || 37 in keysDown || 39 in keysDown)
        moved();
};

// Draw everything
var render = function () {
    if (bgReady) {
        ctx.drawImage(bgImage, 0, 0);
    }

    if (heroReady && (showEnemy || hero == mycharacter)) {
        ctx.drawImage(heroImage, hero.x, hero.y);
    }

    if (monsterReady && (showEnemy || monster == mycharacter)) {
        ctx.drawImage(monsterImage, monster.x, monster.y);
    }

    // Score
    ctx.fillStyle = "rgb(250, 250, 250)";
    ctx.font = "24px Helvetica";
    ctx.textAlign = "left";
    ctx.textBaseline = "top";
    ctx.fillText("Goblins caught: " + monstersCaught, 32, 32);
};

// The main game loop
var main = function () {
    var now = Date.now();
    var delta = now - then;

    if (mycharacter)
        update(mycharacter, delta / 1000);

    render();

    then = now;

    // Request to do this again ASAP
    requestAnimationFrame(main);
};

// Cross-browser support for requestAnimationFrame
var w = window;
requestAnimationFrame = w.requestAnimationFrame || w.webkitRequestAnimationFrame || w.msRequestAnimationFrame || w.mozRequestAnimationFrame;

// Let's play this game!
var then = Date.now();
reset();
main();
var mycharacter = null, enemy = null;