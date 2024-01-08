using LLama.Common;
using LLama;

string projDir = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName;
string modelPath = Path.Combine(projDir, "models", "stable-vicuna-13B.Q2_K.gguf"); // change it to your own model path

Run(modelPath).Wait();

static async Task ChatAsync(string modelPath)
{  
  var prompt = "Transcript of a dialog, where the User interacts with an Assistant named Bob. Bob is helpful, kind, honest, good at writing, and never fails to answer the User's requests immediately and with precision.\r\n\r\nUser: Hello, Bob.\r\nBob: Hello. How may I help you today?\r\nUser: Please tell me the largest city in Europe.\r\nBob: Sure. The largest city in Europe is Moscow, the capital of Russia.\r\nUser:"; // use the "chat-with-bob" prompt here.

  // Load model
  var parameters = new ModelParams(modelPath)
  {
    ContextSize = 1024,
    Seed = 1337,
    GpuLayerCount = 5
  };
  using var model = LLamaWeights.LoadFromFile(parameters);

  // Initialize a chat session
  using var context = model.CreateContext(parameters);
  var ex = new InteractiveExecutor(context);
  ChatSession session = new ChatSession(ex);

  // show the prompt
  Console.WriteLine();
  Console.Write(prompt);

  // run the inference in a loop to chat with LLM
  while (true)
  {
    var result =  session.ChatAsync(prompt, new InferenceParams() { Temperature = 0.6f, AntiPrompts = new List<string> { "User:" } });
    _ = result.ForEachAsync((text, t) =>
    {
      Console.Write(text + " ");
    });
   
    Console.ForegroundColor = ConsoleColor.Green;
    prompt = Console.ReadLine();
    Console.ForegroundColor = ConsoleColor.White;
  }
}

 static async Task Run(string modelPath)
{
  var parameters = new ModelParams(modelPath)
  {
    ContextSize = 1024,
    Seed = 1337,
    GpuLayerCount = 5
  };
  using var model = LLamaWeights.LoadFromFile(parameters);
  using var context = model.CreateContext(parameters);
  var executor = new InteractiveExecutor(context);

  ChatSession session;
  if (Directory.Exists("Assets/chat-with-bob"))
  {
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("Loading session from disk.");
    Console.ForegroundColor = ConsoleColor.White;

    session = new ChatSession(executor);
    session.LoadSession("Assets/chat-with-bob");
  }
  else
  {
    session = new ChatSession(executor);
  }

  session.WithOutputTransform(new LLamaTransforms.KeywordTextOutputStreamTransform(
      new string[] { "User:", "Assistant:" },
      redundancyLength: 8));

  InferenceParams inferenceParams = new InferenceParams()
  {
    Temperature = 0.9f,
    AntiPrompts = new List<string> { "User:" }
  };

  Console.ForegroundColor = ConsoleColor.Yellow;
  Console.WriteLine("The chat session has started.");

  // show the prompt
  Console.ForegroundColor = ConsoleColor.Green;
  string userInput = Console.ReadLine() ?? "";

  while (userInput != "exit")
  {
    if (userInput == "save")
    {
      session.SaveSession("Assets/chat-with-bob");
      Console.ForegroundColor = ConsoleColor.Yellow;
      Console.WriteLine("Session saved.");
    }
    else
    {
      await foreach (var text in session.ChatAsync(userInput, inferenceParams))
      {
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write(text);
      }
    }

    Console.ForegroundColor = ConsoleColor.Green;
    userInput = Console.ReadLine() ?? "";

    Console.ForegroundColor = ConsoleColor.White;
  }
}