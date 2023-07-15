using MongoDB.Bson;

namespace SlotMachine.Entities;

public class Configuration
{
    public ObjectId Id { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }

    public Configuration(int width, int height)
    {
        Id = new ObjectId();
        Width = width;
        Height = height;
    }
}