using System.Text;
using DotnetLibrary.Audience;
using DotnetLibrary.Audience.Factors;
using Godot;

namespace LudumDare54.Audience;

public partial class ActorEffectsRenderer : Node2D
{
    [Export] private AnimatedSprite2D bodySprite;

    [Export] private DebugViewOption debugOption;
    [Export] private RichTextLabel effectText;
    
    public void RenderEffects(CrowdActorEffect effects)
    {
        bodySprite.SelfModulate = Colors.White
            .Lerp(Colors.Red, effects.EnragedEffect);

        if (debugOption == DebugViewOption.EffectAmounts && effectText != null)
        {
            var textBuilder = new StringBuilder();
            textBuilder.Append("[code]");
            textBuilder.AppendFormat("Rage:  {0:0.00}\n", effects.EnragedEffect);
            textBuilder.AppendFormat("Drunk: {0:0.00}\n", effects.DrunkEffect);
            textBuilder.AppendFormat("Alert: {0:0.00}\n", effects.AlertEffect);
            textBuilder.Append("[/code]");
            effectText.Text = textBuilder.ToString();
        }
    }

    public void RenderDebugRawFactors(float[] rawFactors)
    {
        if (debugOption != DebugViewOption.RawFactors || effectText == null) return;

        var textBuilder = new StringBuilder();
        textBuilder.Append("[code]");
        textBuilder.AppendFormat("Rage:   {0:0.00}\n", rawFactors[(int)FactorType.Rage]);
        textBuilder.AppendFormat("Stupor: {0:0.00}\n", rawFactors[(int)FactorType.Stupor]);
        textBuilder.AppendFormat("Horny:  {0:0.00}\n", rawFactors[(int)FactorType.Horny]);
        textBuilder.AppendFormat("Attrac: {0:0.00}\n", rawFactors[(int)FactorType.StinkyToAttractive]);
        textBuilder.Append("[/code]");
        effectText.Text = textBuilder.ToString();
    }
}

public enum DebugViewOption
{
    RawFactors,
    EffectAmounts,
    None
}