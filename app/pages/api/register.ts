// Next.js API route support: https://nextjs.org/docs/api-routes/introduction
import type { NextApiRequest, NextApiResponse } from 'next';

type POST = {
  json: string;
};

export default function handler(req: NextApiRequest, res: NextApiResponse) {
  if (req.method === 'POST') {
    return post(req, res);
  }
}

/*
How to call with curl:  curl -i -X POST -H 'Content-Type: application/json' -d '{"email":"jondas@web.de","referenceId":"test5"}' http://localhost:3000/api/register
*/
const post = async (req: NextApiRequest, res: NextApiResponse<POST>) => {

  console.log("req.body: " + JSON.stringify(req.body)); 
  const { email: email, referenceId: referenceId } = req.body;

    var SHIFT_API_URI = "https://api.gameshift.dev/";

    const apiKey = process.env.SHIFT_API_KEY ? process.env.SHIFT_API_KEY : "";
    const result = await fetch(SHIFT_API_URI + "/users",{
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        "x-api-key": apiKey
      },
      body: JSON.stringify({
        email: email,
        referenceId: referenceId
      })
    });
    const json = await result.json();
    console.log(json);
    res.status(200).send({ json: json });
};
