using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface ISpeechBubble:IDialogueBubble
{
    void SetDialogueBubbleContent(SpeakingLineContent content);
}
