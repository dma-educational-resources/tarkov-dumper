
# EFT DMA Dumper

![icon-static](https://github.com/user-attachments/assets/d2b02f5a-298c-45fd-8154-2331f1f21c0f)

## What is this?
- This is an up-to-date offset dumper/generator for the Lone DMA EFT/Arena built by x0m.

## How do I start using this?
1. Download & extract the solution
2. Open solution with visual studio
3. Modify the `Assembly-CSharp.dll`, `dump.txt` & `SDK.cs` paths in `EFTProcessor.cs` & `ArenaProcessor.cs` to your liking
4. Make the appropriate folder hierarchy mentioned above, also place your Unispect `dump.txt` & game `Assembly-CSharp.dll` in said folders (Default is `C:\lone-dma\eft\` & `C:\lone-dma\arena\`)
3. Run the `TarkovDumper` project in Debug/Release & it will generate an `SDK.cs` for both EFT & Arena in their respective folders

## Credits
- Lone Survivor

## Contact
- For any inquiries or assistance, feel free to join the [DMA Educational Resources Discord Server](https://discord.gg/hxUhJHWuap).