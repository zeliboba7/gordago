/**
* @version $Id: OutputWriter.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.Core
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using System.IO;
  using System.Windows.Forms;

  class OutputWriter : StringWriter {
    private delegate void StringEventHandler(string value);
    private RichTextBox textBox;

    public OutputWriter(RichTextBox textBox) {
      this.textBox = textBox;
    }

    private void Add(string line) {
      if (this.textBox.InvokeRequired) {
        this.textBox.Invoke(new StringEventHandler(this.Add), new object[] { line });
      } else {
        try {
          this.textBox.AppendText(line);
        } catch {
        }
      }
    }

    private void AddLine(string line) {
      this.Add(string.Format("{0}{1}", line, Environment.NewLine));
    }

    public override void Write(char[] buffer) {
      this.Write(new string(buffer));
    }

    public override void Write(bool value) {
      this.Add(value.ToString());
    }

    public override void Write(char value) {
      this.Add(value.ToString());
    }

    public override void Write(decimal value) {
      this.Add(value.ToString());
    }

    public override void Write(double value) {
      this.Add(value.ToString());
    }

    public override void Write(int value) {
      this.Add(value.ToString());
    }

    public override void Write(long value) {
      this.Add(value.ToString());
    }

    public override void Write(float value) {
      this.Add(value.ToString());
    }

    public override void Write(string value) {
      this.Add(value);
    }

    public override void Write(uint value) {
      this.Add(value.ToString());
    }

    public override void Write(ulong value) {
      this.Add(value.ToString());
    }

    public override void WriteLine() {
      try {
        this.textBox.AppendText(Environment.NewLine);
      } catch {
      }
    }

    public override void WriteLine(bool value) {
      this.AddLine(value.ToString());
    }

    public override void WriteLine(char value) {
      this.AddLine(value.ToString());
    }

    public override void WriteLine(decimal value) {
      this.AddLine(value.ToString());
    }

    public override void WriteLine(double value) {
      this.AddLine(value.ToString());
    }

    public override void WriteLine(int value) {
      this.AddLine(value.ToString());
    }

    public override void WriteLine(long value) {
      this.AddLine(value.ToString());
    }

    public override void WriteLine(float value) {
      this.AddLine(value.ToString());
    }

    public override void WriteLine(string value) {
      this.AddLine(value);
    }

    public override void WriteLine(uint value) {
      this.AddLine(value.ToString());
    }

    public override void WriteLine(ulong value) {
      this.AddLine(value.ToString());
    }

  }
}
