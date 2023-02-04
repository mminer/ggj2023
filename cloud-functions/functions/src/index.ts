import * as functions from "firebase-functions";
import * as messaging from "@google-cloud/pubsub";

type BaseRequest = Readonly<{
  topic: string;
  event: string;
  playerId: string;
}>;

type CreateTopicRequest = Readonly<{
  topic: string;
}>;

const projectId = "ggj2023-cbcf5";
const pubsub = new messaging.PubSub({projectId});

export const createTopic = functions.https.onRequest(async (req, resp) => {
  const body: CreateTopicRequest = req.body;

  if (!body || !body.topic) {
    resp.status(400).send("Unexpected request parameters");
    return;
  }

  let topic: messaging.Topic | null | undefined;

  await pubsub.topic(body.topic).create((err, topicRes) => {
    topic = topicRes;

    if (err) {
      functions.logger.error(
          err.message,
          err,
          {structuredData: true}
      );
    }
  });

  if (!topic) {
    functions.logger.info(
        "Cannot create topic",
        body.topic,
        {structuredData: true}
    );
    resp.status(403).send(`Unable to create Topic '${body.topic}'`);
    return;
  }

  functions.logger.info(
      "Created new topic",
      body.topic,
      {structuredData: true}
  );

  resp.send(`Created topic '${body.topic}'`);
});

export const sendMessage = functions.https.onRequest(async (req, resp) => {
  const body: BaseRequest = req.body;

  if (!body || !body.topic || !body.event || !body.playerId) {
    resp.status(400).send("Unexpected request parameters");
    return;
  }

  let topic: messaging.Topic | null | undefined;

  await pubsub.topic(body.topic).get((err, topicRes) => {
    topic = topicRes;

    if (err) {
      functions.logger.error(
          err.message,
          err,
          {structuredData: true}
      );
    }
  });

  if (!topic) {
    functions.logger.info(
        "Cannot find topic",
        body.topic,
        {structuredData: true}
    );
    resp.status(404).send(`Topic '${body.topic}' not found`);
    return;
  }

  await topic.publishMessage({data: JSON.stringify(req.body)});

  functions.logger.info(
      "Published event",
      req.body,
      {structuredData: true}
  );

  resp.send(`Published message to '${body.topic}'`);
});
