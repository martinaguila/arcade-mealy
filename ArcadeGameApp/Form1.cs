using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ArcadeGameApp
{
    public partial class Form1 : Form
    {
        private List<int> states = new List<int>(); // List to keep track of states
        private Button startButton;
        private Button playButton;
        private Button winButton;
        private Button loseButton;
        private TextBox statesTextBox;
        private TextBox instructionsTextBox;

        public Form1()
        {
            InitializeComponent();
            InitializeUI();
            states.Add(0); // Initial state
            UpdateStatesTextBox();
            UpdateButtonStates();
        }

        private void InitializeUI()
        {
            // Create UI elements
            startButton = new Button();
            startButton.Text = "Start";
            startButton.Location = new Point(50, 50);
            startButton.Size = new Size(100, 50);
            startButton.Click += StartButton_Click;
            Controls.Add(startButton);

            playButton = new Button();
            playButton.Text = "Play";
            playButton.Location = new Point(200, 50);
            playButton.Size = new Size(100, 50);
            playButton.Click += PlayButton_Click;
            Controls.Add(playButton);

            // Create Win button
            winButton = new Button();
            winButton.Text = "Win";
            winButton.Location = new Point(350, 50);
            winButton.Size = new Size(100, 50);
            winButton.Click += WinButton_Click;
            Controls.Add(winButton);

            // Create Lose button
            loseButton = new Button();
            loseButton.Text = "Lose";
            loseButton.Location = new Point(500, 50);
            loseButton.Size = new Size(100, 50);
            loseButton.Click += LoseButton_Click;
            Controls.Add(loseButton);

            // Create states text box
            statesTextBox = new TextBox();
            statesTextBox.Location = new Point(50, 120);
            statesTextBox.Size = new Size(300, 80);
            statesTextBox.Multiline = true;
            statesTextBox.ReadOnly = true;
            statesTextBox.ScrollBars = ScrollBars.Vertical;
            Controls.Add(statesTextBox);

            // Create instructions text box
            instructionsTextBox = new TextBox();
            instructionsTextBox.Location = new Point(50, 300); // Adjusted location to prevent overlap
            instructionsTextBox.Size = new Size(600, 100);
            instructionsTextBox.Multiline = true;
            instructionsTextBox.ReadOnly = true;
            instructionsTextBox.ScrollBars = ScrollBars.Vertical;
            instructionsTextBox.Text = "• The game starts in state S0.\n" + Environment.NewLine +
                         "• Upon player input 'Start', the game transitions to S1 (Insert coin).\n" + Environment.NewLine +
                         "• In S1, player 'Action', state is Game started.\n" + Environment.NewLine +
                         "• Winning a level (input 'Win') triggers a transition to S2 (Leveling).\n" + Environment.NewLine +
                         "• Losing the game (input 'Lose') triggers a transition to S3 (Game Over).\n" + Environment.NewLine +
                         "• The machine remains in S2 (Level Up) if a 'Win'.\n" + Environment.NewLine +
                         "• Any input in S3 (Game Over) has no effect (remains in game over state).";
            Controls.Add(instructionsTextBox);
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            // Transition from s0 to s1, or reset from s4 to s0
            if (states[states.Count - 1] == 0)
            {
                states.Add(1);
            }
            else if (states[states.Count - 1] == 4)
            {
                states.Clear();
                states.Add(0);
            }
            UpdateStatesTextBox();
            Refresh();
            UpdateButtonStates();
        }

        private void PlayButton_Click(object sender, EventArgs e)
        {
            // Transition from s1 to s2
            if (states[states.Count - 1] == 1)
            {
                states.Add(2);
                UpdateStatesTextBox();
                Refresh();
                UpdateButtonStates();
            }
        }

        private void WinButton_Click(object sender, EventArgs e)
        {
            // Transition from s2 to s3
            if (states[states.Count - 1] == 2)
            {
                states.Add(3);
                UpdateStatesTextBox();
                Refresh();
                UpdateButtonStates();
            }
        }

        private void LoseButton_Click(object sender, EventArgs e)
        {
            int currentState = states[states.Count - 1];
            // Transition from s2 to s3 and then from s3 to s4
            if (currentState == 2)
            {
                states.Add(3);
                states.Add(4);
            }
            else if (currentState == 3)
            {
                states.Add(4);
            }
            UpdateStatesTextBox();
            Refresh();
            UpdateButtonStates();
        }

        private void UpdateStatesTextBox()
        {
            statesTextBox.Clear();
            foreach (int state in states)
            {
                statesTextBox.AppendText(GetStateDescription(state) + Environment.NewLine);
            }
        }

        private void UpdateButtonStates()
        {
            int currentState = states[states.Count - 1];
            startButton.Enabled = (currentState == 0 || currentState == 4);
            playButton.Enabled = (currentState == 1);
            winButton.Enabled = (currentState == 2);
            loseButton.Enabled = (currentState == 2 || currentState == 3);
        }

        private string GetStateDescription(int state)
        {
            switch (state)
            {
                case 0:
                    return "s0: Idle";
                case 1:
                    return "s1: Coin inserted";
                case 2:
                    return "s2: Game started";
                case 3:
                    return "s3: Leveling";
                case 4:
                    return "s4: Game over";
                default:
                    return "Unknown state";
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Draw states
            Graphics g = e.Graphics;
            Font font = new Font("Arial", 14, FontStyle.Bold);
            Brush brush = new SolidBrush(Color.Black);
            Pen pen = new Pen(Color.Black, 2);
            int x = 100; // Initial X-coordinate for drawing states
            int y = 250; // Y-coordinate for drawing states (adjusted to accommodate text box)
            int circleRadius = 30; // Radius of the circle

            for (int i = 0; i < 5; i++)
            {
                // Draw light brown filled circle
                g.FillEllipse(new SolidBrush(Color.BurlyWood), x - circleRadius, y - circleRadius, circleRadius * 2, circleRadius * 2);
                g.DrawEllipse(pen, x - circleRadius, y - circleRadius, circleRadius * 2, circleRadius * 2);

                // Measure the size of the string
                SizeF stringSize = g.MeasureString("s" + i, font);
                // Calculate the position to center the text
                float textX = x - stringSize.Width / 2;
                float textY = y - stringSize.Height / 2;

                // Draw the state text
                g.DrawString("s" + i, font, brush, new PointF(textX, textY));
                x += 120; // Increase x-coordinate for the next state
            }

            // Draw arrows for state transitions
            if (states.Count > 1)
            {
                Pen arrowPen = new Pen(Color.Black, 3);
                for (int i = 0; i < states.Count - 1; i++)
                {
                    int startX = 100 + (i * 120) + circleRadius;
                    int endX = 100 + ((i + 1) * 120) - circleRadius;
                    g.DrawLine(arrowPen, new Point(startX, y), new Point(endX, y));
                    DrawArrowHead(g, arrowPen, new Point(endX, y));
                }
            }
        }

        // Method to draw arrowhead
        private void DrawArrowHead(Graphics g, Pen pen, Point endPoint)
        {
            const int arrowSize = 10;
            Point[] arrowPoints = new Point[]
            {
        new Point(endPoint.X - arrowSize, endPoint.Y - arrowSize),
        new Point(endPoint.X - arrowSize, endPoint.Y + arrowSize),
        endPoint
            };
            g.FillPolygon(Brushes.Black, arrowPoints);
        }
    }
}