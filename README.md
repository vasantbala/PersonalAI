# PersonalAI

## Getting started
1. Download the latest release from https://github.com/vasantbala/PersonalAI/releases
2. Extract the contents to a folder of your choice
3. Edit appsettings.json to add your llm service like below
   ```json
   {
    "AppSettings": {
    "GradleSettings": [
      {
        "LLMType": "My LLM",
        "Url": "http://127.0.0.1:5000"
      }
    ]
    }
   }
  ```
