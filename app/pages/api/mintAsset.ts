// Next.js API route support: https://nextjs.org/docs/api-routes/introduction
import type { NextApiRequest, NextApiResponse } from 'next';

type POST = {
  json: string;
};

export default function handler(req: NextApiRequest, res: NextApiResponse) {
  if (req.method === 'POST') {
    return post(req, res);
  }
  res.setHeader('Access-Control-Allow-Credentials', "true")
  res.setHeader('Access-Control-Allow-Origin', '*')
  res.setHeader('Access-Control-Allow-Methods', 'GET,OPTIONS,PATCH,DELETE,POST,PUT')
  res.setHeader(
    'Access-Control-Allow-Headers',
    'X-CSRF-Token, X-Requested-With, Accept, x-api-key, Accept-Version, Content-Length, Content-MD5, Content-Type, Date, X-Api-Version'
  )
  if (req.method === 'OPTIONS') {
    res.status(200).end()
    return
  }
}

/*
How to call with curl: curl -i -X POST -H 'Content-Type: application/json' -d '{"assetId":"Monkey","referenceId":"jonas"}' http://localhost:3000/api/mintAsset
*/
const post = async (req: NextApiRequest, res: NextApiResponse<POST>) => {

  const { assetId, referenceId } = req.body;

    var SHIFT_API_URI = "https://api.gameshift.dev/";

    var nftJsonPayload = "";
    console.log("req.body: " + JSON.stringify(req.body));
    switch (assetId) {
      case "Monkey":
        nftJsonPayload = generateMonkeyJson(referenceId);
        break;
      case "FireMage":
        nftJsonPayload = generateFireMageJson(referenceId);
        break;
      default:
        console.log("No asset for assetId: " + assetId + " found");
        return res.status(404).send({ json: "No asset for that asset id." });
    }

    const apiKey = process.env.SHIFT_API_KEY ? process.env.SHIFT_API_KEY : "";
    const result = await fetch(SHIFT_API_URI + "/assets",{
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        "x-api-key": apiKey
      },
      body: nftJsonPayload
    });
    const json = await result.json();

    res.status(200).send({ json: json });
};

function generateMonkeyJson(destinationUserReferenceId: string) {
  const jsonString = `
  {
      "details": {
          "attributes": [
              {
                  "traitType": "Banana",
                  "value": "3"
              }
          ],
          "collectionId": "eaf23fb5-7030-4cea-b065-5615597993df",
          "description": "Starts with 3 extra rotating Bananas.",
          "imageUrl": "https://shdw-drive.genesysgo.net/AzjHvXgqUJortnr5fXDG2aPkp2PfFMvu4Egr57fdiite/Monkey.png",
          "name": "Monkey"
      },
      "destinationUserReferenceId": "${destinationUserReferenceId}"
  }`;

  return jsonString;
}

function generateFireMageJson(destinationUserReferenceId: string) {
  const jsonString = `
  {
      "details": {
          "attributes": [
              {
                  "traitType": "FireBall",
                  "value": "1"
              },
              {
                  "traitType": "FireBolt",
                  "value": "2"
              }
          ],
          "collectionId": "eaf23fb5-7030-4cea-b065-5615597993df",
          "description": "Starts with 3 extra fire Spells.",
          "imageUrl": "https://shdw-drive.genesysgo.net/AzjHvXgqUJortnr5fXDG2aPkp2PfFMvu4Egr57fdiite/FireMage.png",
          "name": "Fire Mage"
      },
      "destinationUserReferenceId": "${destinationUserReferenceId}"
  }`;

  return jsonString;
}
