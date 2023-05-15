namespace player_api_v2
{
  public class StateModel
  {
    public string state { get; set; }
    public string uniqueId { get; set; }

    public void GenerateUniqueId()
    {
      // Generate a new UUID
      uniqueId = Guid.NewGuid().ToString();
    }

    public string GetRandomState()
    {
      // Define the available states
      string[] states = { "State1", "State2", "State3", "State4", "State5", "State6" };

      // Generate a random index
      Random random = new Random();
      int index = random.Next(0, states.Length);

      // Return the randomly selected state
      return states[index];
    }
  }
}
