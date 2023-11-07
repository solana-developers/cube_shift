# cube_shift

Cube Shift shows a simple implementation of the Solana Game Shift Api. 

The game is similar to vampire survivors or blob heroes and is written in Unity game engine. 

<img width="1832" alt="image" src="https://github.com/solana-developers/cube_shift/assets/5938789/966c39e7-a461-4275-a34e-7ee29318ab5a">

Here is a live version of the game: 
https://solplay.de/cubeshift/

The game has a next.js api that communicates with the Solana Labs Game Shift api. 
From Unity it calls a next js app deployed in vercel which forwards the requests to the shift api using an API key. 

Signup to get your own api key here: 
https://gameshift.solanalabs.com/
https://app.gameshift.dev/

Features of game shift used: 
- Sign up player using a client token and email address
- Minting assets in form of new characters for the game. Fire mage which has extra fire bolts and Monkey which starts with extra banana skill. 

Players login with email adress and the access token is saved in the client for now. 

Feel free to fork the game and use it or open PRs right here. 

