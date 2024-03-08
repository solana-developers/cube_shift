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

<img width="892" alt="image" src="https://github.com/solana-developers/cube_shift/assets/5938789/37ac970e-501d-43ce-8315-d8f5e7a11dad">


Players login with email adress and the access token is saved in the client for now. 

Feel free to fork the game and use it or open PRs right here. 


# Integrating GameShift with Unity

This document provides guidelines on integrating GameShift, into your Unity game to handle transactions and game state securely and efficiently.

## Recommended Game Architecture

When integrating GameShift into your Unity game, we recommend the following architecture:

![Gameshift client and server architecture](https://ff56da2b343128e8a376a0b81f448acdd42bdbc3ae4326d6a06a164-apidata.googleusercontent.com/download/storage/v1/b/gameshift-unity-architecture/o/archi.png?jk=ATxpoHcAx_epkO2K2ScZmQ3OAUcjuD3jsPWBNb4JqHuoxtA_e0xJe7HIxKTQO7y-yDAbmSWliiumBgUFsNriFKUuUlnaxRYMG8zT9-KJt4oZ9Ap2MpgE-Vlgpn1RJ5Tiqi62QdZVrTd7YOxSghHwLiGMN3GLHyJi-tdeUmQUHAqNmcQmfritf9F7hTmpCxQEoB19GNQIIMQUtkDgdheLktppJlNuTYfzyz8RFdnyd5RoU7NpDUZef6WDKAcMPCEWKr1QnlEbP1BsZfuFpWnTBe-34oEfdflyFTJU8xnSVXU7iIVGlyGUWxc90XQGQOBPr_GB7rnt5fhfzO6aKl_tHv4ucNbv08-z8pjfg_GXivG8PPj77dwNTxo1bTWHHP6T_kc1l24Nt4iD4-WyPrUGK2FXv_B3vQ7s427nPWi8XC6Sj0GRxa_5WF1_XrlHycoSCML-sDiD8E_0W6AiPY2Imx1tKnN7pBCtbshGA1ikjBYiIh2l6er4VmwZWP4i5d0K-Qjq6JTktbpoIcEI6lyZIf6nlgP7WZpysnXRASaZwb_iQAzUmt_gy_afPkR3CmCxgBpWe9MNqHV-mJ3V6Qp5pDb_dkubHFyEw6WU_yTPtpYbH7TLfhLVXjHU1CIOhXLvItaN0nqNwjiMVYuu1XIAXUznBwAFBTNlllwqjtmZBjOdKwUvYHrOr7qsyBYsmuBhaTBhJVyzXbfChh1962Px7oAKUn1qzteuPNaWPI7Mfc4k45D86L0tZQ9b7z_rmkeP8EpljFRCby8Z88pNFosJhj4J7Ak5l0wUwyKGxkqD-y-JxV2H3hlkZqf1-IkXywo1m1CauocMifhbmxlf02_eJ-USthTTrINIQqs1HtEuKA_R_9TXAGPuqzJjafXsE9nYgr9dp0uwX3zhS8CZKdgxQRhHKKCJJg1JT3pnrvd5LouIe6lHJUiEtEzbN79ccfDiNeyyxf9NwVkFAxOl3KITd6buYIz_4BuR87Y1pl6cXxxrCcBumR2E7-wIFdelubCNmjTSiydTKwK5DNawoPLaEfS5KpPwDbzaWP_wVQu6KUsOlXrhOvTxD5IFZqVoWOqRfNjIP_wc-5pY_jdA1JE5KDvLrlBTNUJDO50HyUGgAd7oAFLnewjIc_gDNfiB4i4zVdwqg5rAdZ_O1ZFKQDo31k5JBHERAetQUPdhXlKgP4fbdqgYcfVK5mPb&isca=1)

- **Game Client:** A Unity application that the player interacts with. It is responsible for presenting the game state and handling user inputs.
- **Server:** Your custom backend server acts as an intermediary between the Game Client and GameShift API. It is crucial for several reasons:
  - *Security:* The server ensures that sensitive GameShift API calls are not exposed to the client, reducing the risk of unauthorized access and manipulation.
  - *User Profiling:* Stores persistent user data that lives with the game, such as profiles, progress, and statistics.
  - *Custom Game Logic:* Allows the implementation of custom game logic that is not handled by the Game Client or GameShift API. Utilizing a server will make this data processing faster than in Unity.
 - **GameShift:** Gameshift will contain the canonical on-chain data for your game

The server plays a major role in this architecture by processing game logic, securely interacting with the GameShift API, and updating the game state based on the responses.

## Workflow

The interaction between the Game Client, Server, and GameShift API typically follows these steps. We've included sample code to demonstrate a potential integration—the github repo is available [here](https://github.com/harshasomisetty/cube_crusher/tree/main), we will follow sample code to show how a player might be awarded an nft after clearing a level

1. **Action Sending:** The Game Client sends player actions to the Server. [code](https://github.com/harshasomisetty/cube_crusher/blob/438d3047d4b86a3a4c5ec958b642a0490cbef2c4/3dgame/Assets/Scripts/Data/NetworkService.cs#L49)

2. **Data Processing:** The Server processes these actions and sends relevant requests to the GameShift API, and receives an appropriate response. [code](https://github.com/harshasomisetty/cube_crusher/blob/438d3047d4b86a3a4c5ec958b642a0490cbef2c4/server/src/routes/user.ts#L156)

3. **State Update:** The Server updates the game state and sends it back to the Game Client. [code](https://github.com/harshasomisetty/cube_crusher/blob/438d3047d4b86a3a4c5ec958b642a0490cbef2c4/3dgame/Assets/Scripts/Menus/EndMenu.cs#L65)
