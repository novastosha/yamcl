namespace YAMCL

open Elmish
open Avalonia.FuncUI.Elmish.ElmishHook
open Avalonia.FuncUI.Hosts
open Avalonia.FuncUI.DSL
open Avalonia.Controls
open Avalonia.FuncUI

type Page =
  | MainPage
  | NewInstancePage

module Shell =
  open Avalonia.FuncUI.Types
  open Avalonia.Layout
  open Avalonia

  type State = { CurrentPage: Page }

  type Msg = SetPage of Page

  let init () = { CurrentPage = MainPage }, Cmd.none

  let update msg state =
    match msg with
    | SetPage page -> { state with CurrentPage = page }, Cmd.none

  let mainPageView (navigateToPage) = 
      Component.create ("main", fun ctx -> 
        let state, dispatch = ctx.useElmish (init, (update))

        let toolbarView () = 
            let buttons = [
                ("Create New Instance", (fun () -> navigateToPage NewInstancePage))
            ]

            Border.create [
                Grid.row 0
                Border.background "#323232"
                Border.borderThickness 0.75
                Border.borderBrush "#838383"
                Border.cornerRadius 0 
                Border.child (
                    StackPanel.create [
                        StackPanel.orientation Orientation.Horizontal
                        StackPanel.horizontalAlignment HorizontalAlignment.Left
                        StackPanel.children [
                            yield! buttons 
                            |> List.map (fun (name,clickFunction) -> 
                            Button.create [
                                Button.content name
                                Button.onClick (fun _ -> clickFunction ())
                                Button.fontSize 13
                                Button.padding 0
                                Button.cornerRadius 0
                                Button.background "Transparent"
                                Button.margin (Thickness(10, 0, 0, 0))
                                Button.padding (Thickness(10,0,10,0))
                                Grid.row 1
                                ] :> IView) // horrible formatting
                        ]
                    ]
                )
            ]

        Grid.create [
            Grid.rowDefinitions "Auto,*"
            Grid.children [
                toolbarView ()
                
            ]
        ]
    )

  let view () =
    Component(fun ctx ->
      let state, dispatch = ctx.useElmish (init, update)

      Grid.create [
        Grid.rowDefinitions "*,Auto,Auto"
        
        Grid.children [
            let mutable toolbarButtons = []
            let addToolbarButton (name: string) (func:(unit -> unit)) =
                toolbarButtons <- (name, func) :: toolbarButtons 

            addToolbarButton "Back" (fun () -> dispatch (SetPage MainPage))

            match state.CurrentPage with
            | MainPage -> mainPageView (SetPage >> dispatch)
            | NewInstancePage -> NewInstancePage.view (fun () -> dispatch (SetPage MainPage)) addToolbarButton

            match state.CurrentPage with
            | MainPage -> ()
            | _ -> Border.create [
                    Grid.row 1
                    Border.background "#323232"
                    Border.borderThickness 0.75
                    Border.borderBrush "#838383"
                    Border.cornerRadius 0 
                    Border.child (
                        StackPanel.create [
                            StackPanel.orientation Orientation.Horizontal
                            StackPanel.horizontalAlignment HorizontalAlignment.Right
                            StackPanel.children [
                                yield! toolbarButtons 
                                |> List.map (fun (name,clickFunction) -> 
                                Button.create [
                                          Button.content name
                                          Button.onClick (fun _ -> clickFunction ())
                                          Button.fontSize 13
                                          Button.padding 0
                                          Button.cornerRadius 0
                                          Button.background "Transparent"
                                          Button.margin (Thickness(10, 0, 0, 0))
                                          Button.padding (Thickness(10,0,10,0))
                                          Grid.row 1
                                        ] :> IView) // horrible formatting
                            ]
                        ]
                    )
                ]
            Border.create [
              Grid.row 2
              Border.background "#323232"
              Border.borderThickness 0.75
              Border.borderBrush "#838383"
              Border.cornerRadius 0 
              Border.child (
                Grid.create [
                  Grid.columnDefinitions "*,*"
                  Grid.children [
                    let basicText text column alignment = 
                      TextBlock.create [
                        Grid.column column
                        TextBlock.fontSize 12.65
                        TextBlock.horizontalAlignment alignment
                        TextBlock.text text
                      ]

                    basicText "Version: 1.0.0-release-git" 0 HorizontalAlignment.Left
                    basicText "Lorem ipsum dolor sit." 1 HorizontalAlignment.Right
                  ]
                ]
              )
            ]

        ]
      ])

  type MainWindow() as host =
    inherit HostWindow()

    do
      base.Title <- "Yet Another Minecraft Launcher"
      base.CanResize <- true

      base.Width <- 1200
      base.Height <- 600

      base.MinWidth <- 800
      base.MinHeight <- 600
      base.WindowStartupLocation <- WindowStartupLocation.CenterOwner

      host.Content <- view ()