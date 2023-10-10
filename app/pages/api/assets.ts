// Next.js API route support: https://nextjs.org/docs/api-routes/introduction
import type { NextApiRequest, NextApiResponse } from 'next';

type GET = {
  json: any;
};

export default function handler(req: NextApiRequest, res: NextApiResponse) {
  if (req.method === 'GET') {
    return get(req, res);
  }
}

function getFromPayload(req: NextApiRequest, payload: string, field: string): string {
  function parseError() { throw new Error(`${payload} parse error: missing ${field}`) };
  let value;
  if (payload === 'Query') {
    if (!(field in req.query)) parseError();
    value = req.query[field];
  }
  if (payload === 'Body') {
    if (!req.body || !(field in req.body)) parseError();
    value = req.body[field];
  }
  if (value === undefined || value.length === 0) parseError();
  return typeof value === 'string' ? value : value[0];
}

const get = async (req: NextApiRequest, res: NextApiResponse<GET>) => {

    var SHIFT_API_URI = "https://api.gameshift.dev/";
    const user = getFromPayload(req, 'Query', 'user');

    const apiKey = process.env.SHIFT_API_KEY ? process.env.SHIFT_API_KEY : "";
    const result = await fetch(SHIFT_API_URI + "/users/" + user + "/assets",{
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        "x-api-key": apiKey
      }
    });
    const json = await result.json();
    res.status(200).json({
      json: json,
    });
};
