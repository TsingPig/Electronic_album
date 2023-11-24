//-----------------------------------------------------------------------
// <copyright file="InlineButtonAttributeDrawer.cs" company="Sirenix IVS">
// Copyright (c) Sirenix IVS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Editor.Drawers
{
    using Sirenix.OdinInspector;
    using Sirenix.OdinInspector.Editor;
    using Sirenix.OdinInspector.Editor.ActionResolvers;
    using Sirenix.OdinInspector.Editor.ValueResolvers;
    using Sirenix.Utilities;
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// Draws properties marked with <see cref="InlineButtonAttribute"/>
    /// </summary>
    [DrawerPriority(DrawerPriorityLevel.WrapperPriority)]
    public sealed class InlineButtonAttributeDrawer<T> : OdinAttributeDrawer<InlineButtonAttribute, T>
    {
        private ValueResolver<string> labelGetter;
        private ActionResolver clickAction;
        private ValueResolver<bool> showIfGetter;

        private bool show = true;

        protected override void Initialize()
        {
            if (this.Attribute.Label != null)
            {
                this.labelGetter = ValueResolver.GetForString(this.Property, this.Attribute.Label);
            }
            else
            {
                this.labelGetter = ValueResolver.Get<string>(this.Property, null, Attribute.Action.SplitPascalCase());
            }

            this.clickAction = ActionResolver.Get(this.Property, this.Attribute.Action);

            this.showIfGetter = ValueResolver.Get(this.Property, this.Attribute.ShowIf, true);

            this.show = this.showIfGetter.GetValue();
        }

        /// <summary>
        /// Draws the property.
        /// </summary>
        protected override void DrawPropertyLayout(GUIContent label)
        {
            if (this.labelGetter.HasError || this.clickAction.HasError)
            {
                this.labelGetter.DrawError();
                this.clickAction.DrawError();
                this.CallNextDrawer(label);
            }
            else
            {
                if (Event.current.type == EventType.Layout)
                {
                    this.show = this.showIfGetter.GetValue();
                }

                if (this.show)
                {
                    EditorGUILayout.BeginHorizontal();

                    EditorGUILayout.BeginVertical();
                    this.CallNextDrawer(label);
                    EditorGUILayout.EndVertical();

                    string buttonLabel = this.labelGetter.GetValue();
                    if (GUILayout.Button(buttonLabel, EditorStyles.miniButton, GUILayoutOptions.ExpandWidth(false).MinWidth(20)))
                    {
                        this.Property.RecordForUndo("Click " + buttonLabel);
                        this.clickAction.DoActionForAllSelectionIndices();
                    }

                    EditorGUILayout.EndHorizontal();
                }
                else
                {
                    this.CallNextDrawer(label);
                }
            }
        }
    }
}
#endif