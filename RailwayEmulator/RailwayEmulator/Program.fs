// Learn more about F# at https://fsharp.org
// See the 'F# Tutorial' project for more help.

open System.Windows.Forms

let form = new Form(Text = "Railway Emulator")
form.Visible <- true
form.Text <- "Emulator"

let label = new Label()
label.Text <- "Emulator"

form.Controls.Add(label)

//[<STAThread>]
Application.Run(form)

//let button = new Button(Text = "Close", Dock = DockStyle.Fill)
//button.Click.Add(fun _ -> Application.Exit() |> ignore)
//form.Controls.Add(button)
//form.Show()

//[<EntryPoint>]
//let main argv =
//    printfn "%A" argv
//    0 // return an integer exit code
