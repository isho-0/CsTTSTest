using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Speech.Synthesis;

namespace CsTTSTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SpeechSynthesizer speechSynthesizer;
        public MainWindow()
        {
            InitializeComponent();
            InitializeSpeechSynthesizer();
            InitializeLanguageComboBox();
        }
        private void InitializeLanguageComboBox()
        {
            selectLanguage.Items.Add("한국어");
            selectLanguage.Items.Add("영어");
            selectLanguage.SelectedIndex = 0;
        }

        private void InitializeSpeechSynthesizer()
        {
            speechSynthesizer = new SpeechSynthesizer();
            speechSynthesizer.SetOutputToDefaultAudioDevice();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (selectLanguage.SelectedItem == null)
                return;

            string selectedLanguage = selectLanguage.SelectedItem.ToString();

            var voices = speechSynthesizer.GetInstalledVoices();

            if (selectedLanguage == "한국어")
            {
                var koreanVoice = voices.FirstOrDefault(v =>
                    v.VoiceInfo.Culture.TwoLetterISOLanguageName == "ko");

                if (koreanVoice != null)
                    speechSynthesizer.SelectVoice(koreanVoice.VoiceInfo.Name);
            }
            else if (selectedLanguage == "영어")
            {
                var englishVoice = voices.FirstOrDefault(v =>
                    v.VoiceInfo.Culture.TwoLetterISOLanguageName == "en");

                if (englishVoice != null)
                    speechSynthesizer.SelectVoice(englishVoice.VoiceInfo.Name);
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string textToSpeak = inputText.Text.Trim();

            if (string.IsNullOrEmpty(textToSpeak))
            {
                MessageBox.Show("텍스트를 입력해주세요.", "알림");
                return;
            }

            Task.Run(() =>
            {
                speechSynthesizer.Speak(textToSpeak);
            });
        }

        protected override void OnClosed(EventArgs e)
        {
            if(speechSynthesizer != null)
            {
                speechSynthesizer.Dispose();
            }
            base.OnClosed(e);
        }
    }
    
}
