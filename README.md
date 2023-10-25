# SeanCopilot
This is a simple Notepad++ plugin which adds OpenAI API to Notepad++, specifcally for helping to write code.

Based on the plugin template found here: https://github.com/kbilsted/NotepadPlusPlusPluginPack.Net

This plugin may have bugs - it was written in a short period of time without much polishing! I use it all the time though and it does a great job helping me write complex (but easy to explain) code fast! I recommend using GPT-4 for doing more complex/larger coding tasks.

# Quick Install
1. Grab the DLL from SeanCopilot/bin/Release
2. Put into your Notepad++ plugins folder (Should look something like C:/Program Files (x86)/Notepad++/plugins/SeanCopilot/SeanCopilot.dll)
Note: 32-bit version also available, but relatively untested.

# How to use
1. Fill in your API key in the settings page and click Save.
2. Choose your model on the settings page (currently, only the chat GPT models have been tested by me)
3. On the main Copilot page, enter a prompt and press Send to get a response from your chosen GPT model
  
# Tips
1. Highlight text before pressing Send to have the selected code refactored by chatGPT (automatically added to the end of your prompt). This will replace the highlighted code with code written by chatGPT!
2. Choose whether or not to maintain the conversation history with ChatGPT - history is useful for contextual requests but can slow down response times (there's probably a maximum size you can send)
3. Write your own instructional prompt for ChatGPT on the settings page. The one I have been working with is included by default, but there are probably much better ways to engineer this prompt.
