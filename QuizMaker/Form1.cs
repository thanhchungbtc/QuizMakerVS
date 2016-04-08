using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using QuizMaker.Model;
using System.IO;

namespace QuizMaker {
    public partial class Form1 : Form {

        // Main datasource
        List<Question> questions;
        List<Question> originalQuestions;
        // To keep track the modified questions
        List<int> modifiedQuestionIds = new List<int>();

        Question _selectedQuestion;
        public Question SelectedQuestion {
            get {
                return _selectedQuestion;
            }
            set {
                _selectedQuestion = value;
                updateDetailView(_selectedQuestion);
            }
        }

        public Form1() {
            InitializeComponent();
            initializeData();
        }

        private void Form1_Load(object sender, EventArgs e) {            
            loadGridQuestion();
        }

        // Read data from json
        void initializeData() {
            string jsonText = File.ReadAllText(Constants.DATA_PATH);
            this.questions = JsonConvert.DeserializeObject<List<Question>>(jsonText);
            saveTemporaryData();
        }

        void saveTemporaryData() {            
            this.originalQuestions = questions.ConvertAll<Question>(q => {
                return q.Clone();
            });
            modifiedQuestionIds.Clear();
        }

        // Save data to json
        void saveData() {

        }

        // Load grid questions
        void loadGridQuestion() {
            foreach (var question in this.questions) {
                grvQuestions.Items.Add(question);
            }
            //grvQuestions.DataSource = questions;
            //grvQuestions.DisplayMember = "Description";
            //grvQuestions.ValueMember = "Id";
        }

        void loadGridOptions(Question question) {
            grvOptions.Items.Clear();    
            foreach (Option option in question.Options) {
                grvOptions.Items.Add(option, option.IsCorrectAnswer);
            }
        }

        void updateDetailView(Question question) {
            if (question == null) {
                txtQuestion.Text = "";
                configureControlsEnable(false);
                grvOptions.Items.Clear();
                return;
            }
            configureControlsEnable(true);            
            txtQuestion.Text = question.Description;
            loadGridOptions(question);
        }

        void configureControlsEnable(bool enable) {
            txtQuestion.Enabled = enable;
            btnReset.Enabled = enable;
        }

        void addToModifiedCollection(int index) {
            modifiedQuestionIds.Add(index);
        }

        #region "Handler events"
        private void grvQuestions_SelectedIndexChanged(object sender, EventArgs e) {
            SelectedQuestion = (Question)grvQuestions.SelectedItem;                        
        }            

        private void grvQuestions_DrawItem(object sender, DrawItemEventArgs e) {
            bool isSelected = ((e.State & DrawItemState.Selected) == DrawItemState.Selected);

            if (e.Index > -1) {              
                Color color = isSelected ? SystemColors.Highlight :
                    this.modifiedQuestionIds.Contains(e.Index) ? Color.LightBlue : Color.White;

                // Background item brush
                SolidBrush backgroundBrush = new SolidBrush(color);
                // Text color brush
                SolidBrush textBrush = new SolidBrush(e.ForeColor);

                // Draw the background
                e.Graphics.FillRectangle(backgroundBrush, e.Bounds);
                // Draw the text
                e.Graphics.DrawString( grvQuestions.Items[e.Index].ToString(), e.Font, textBrush, e.Bounds, StringFormat.GenericDefault);

                // Clean up
                backgroundBrush.Dispose();
                textBrush.Dispose();
            }
            e.DrawFocusRectangle();
        }
        
        private void btnReset_Click(object sender, EventArgs e) {           
            SelectedQuestion.Description = originalQuestions[grvQuestions.SelectedIndex].Description;
            grvQuestions_SelectedIndexChanged(grvQuestions, e);            
        }

        private void txtQuestion_KeyUp(object sender, KeyEventArgs e) {
            int selectedIndex = grvQuestions.SelectedIndex;
            if (selectedIndex < 0 || selectedIndex > this.questions.Count - 1) return;
            addToModifiedCollection(selectedIndex);       
            SelectedQuestion.Description = txtQuestion.Text;
        }

        private void grvOptions_ItemCheck(object sender, ItemCheckEventArgs e) {
            CheckedListBox clb = (CheckedListBox)sender;
            // Switch off event handler
            clb.ItemCheck -= grvOptions_ItemCheck;
            clb.SetItemCheckState(e.Index, e.NewValue);
            // Switch on event handler
            clb.ItemCheck += grvOptions_ItemCheck;

            // Now you can go further
            var options = SelectedQuestion.Options;
            bool value = e.NewValue == CheckState.Checked ? true : false;
            if(options[e.Index].IsCorrectAnswer != value) {
                addToModifiedCollection(grvQuestions.SelectedIndex);
                options[e.Index].IsCorrectAnswer = value;
            }            
        }

        private void btnSave_Click(object sender, EventArgs e) {
            File.WriteAllText(Constants.DATA_PATH, JsonConvert.SerializeObject(this.questions));
            saveTemporaryData();
        }

        private void btnAddQuestion_Click(object sender, EventArgs e) {
            Question question = new Question {
                Id = grvQuestions.Items.Count
            };
            this.questions.Add(question);
            this.grvQuestions.Items.Add(question);
            grvQuestions.SelectedIndex = this.questions.Count - 1;
        }
        #endregion


    }
}
