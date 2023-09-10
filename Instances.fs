namespace YAMCL

open Avalonia.FuncUI.DSL
open Avalonia.Layout
open Avalonia.Controls

type Instance = {
    Version: string
}

module Instance = ()

type InstanceGroup = {
    Name: string
    Instances: Instance list
    IsExpanded: bool
}

module InstanceGroup = 
    open Avalonia.Controls.Primitives
    
    let template (expander: Expander) (nameScope: INameScope) =
        let grid = Grid() 
        
        let button = (ToggleButton())
        button.Content <- expander.Header
        button.Background <- expander.Background
        button.BorderBrush <- expander.BorderBrush
        button.BorderThickness <- expander.BorderThickness
        button.Padding <- expander.Padding
        button.HorizontalAlignment <- HorizontalAlignment.Left
        button.VerticalAlignment <- VerticalAlignment.Stretch
        button.Click.Add(fun _ -> expander.IsExpanded <- not expander.IsExpanded)
        
        grid.Children.Add button

        let instancePanel = StackPanel()


        grid :> Control

    open Avalonia.Controls.Templates
    type InstanceGroupExpander() =
        inherit FuncControlTemplate<Expander>(template)

    let view (group: InstanceGroup) =
        Expander.create [
            Expander.template (InstanceGroupExpander())
            Expander.content group.Instances
        ]

module NewInstancePage = 
    type State = {
        Instance: Instance option
    }

    let view (backToMainPage: unit -> unit)  addToolbarButton =
    
        Grid.create [
        ]