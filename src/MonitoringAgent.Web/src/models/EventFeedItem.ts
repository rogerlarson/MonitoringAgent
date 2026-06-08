export type EventFeedItem =
{
    id: string;
    type: "up" | "down";
    timestamp: string;
    text: string;
};