using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using Application = System.Windows.Application;
using Button = System.Windows.Controls.Button;
using Cursors = System.Windows.Input.Cursors;
using HorizontalAlignment = System.Windows.HorizontalAlignment;
using Label = System.Windows.Controls.Label;
using Orientation = System.Windows.Controls.Orientation;
using Panel = System.Windows.Controls.Panel;

namespace CloudTechWidget
{
    public class Program
    {
        [STAThread]
        public static void Main()
        {
            var app = new Application();
            app.Run(new WidgetWindow());
        }
    }

    public class WidgetWindow : Window
    {
        private NotifyIcon trayIcon;
        private bool isPinned = true;
        private TextBlock pinText;

        // 公式CloudTechロゴ (Base64埋め込みでexe単体でも常に完全表示)
        private const string LogoBase64 = "iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAOWSURBVFhH7dZvbBNlHAdwVKImmviPqNEoODSwMNjqRlel68YGQwNuDIarwNAoiQbYFBYzCKIxZgkJMUNINIUeXa+l13Yb61rG2EaYRDvmQmcEHAsvMIQY0ASz9rn/N+5r+gJTn65hLeirft49v+/vd8ld7rnnZszIyspK0+XLJx5Sh1s/xM+Ox+nsfyGPOqvV4FpRDu9tTKzr18NP4+L+FxEbn5VYv2fk7z/L1Xo2fSOGv5qtRGxWADPjdeVkwwaFWz4kMItVyZ4P8YhRUH0rTsv9H6+hr3FXhJPbjVqg5qY0uHtZfA3gfslb5UGgCPAYELW9eoPYi6/EbAYJ/kWA3wDBX3uIvk7GADyQuBZ8q48gtBiK981j6pkvLAAeBnAf/9vxZ5XerZtl1nwVgXzE/NZ9iXMZi4799JR+/J0thKvaofQ1WtGxBGJX3R667zZy4/wzElfxCzgj5LOtuXSeFmHou+dlz9Jr6DSBOMwEHeXguVX9dB9tYoTJueVbAt63upXO0iJ4q1omuddATuyoAPCgOrjbLI98O4/um0rUUe4lzqXn6HpaYp7q/cS7lqXrqYgR+2x1YBunBup2isc27iLuyrN0z7RMXJ14YrLb+q4WWhfhA+/Z6DyV+A4RBxrNsm/lKNqLwXM1XXTPHUWDDW9Nei1/6t4yaN21+5RTuxbRPXcyCMwU3WUjt7yFGunZup7OUxJP7SnRPEYobHGUDB1cSOfp4IcP5GusAVKbieiXPM/ReZL4Xo6xy0bhXwgS2rKBzjMRc5SG0b4AfEfdTjpLoke+flllCsAfLpoA/niUzjNBuFUMuHng3ZWDdJZEGmgqgzsfUbvpWvxlovNMEPcbTrheAc9W/EpnSRBuma8wBRAZgypd4ubQeSaijtIxtM0FcVX+SGdJ4t98wpaPxR8ZaX972lsvlVhvc7XuNACuXAjttV/S+ZTk/qaVOFoEsHkQA/Wfxw+a25l+0ffkX+dsj/17YmrSBW6u7LL8jkNzILW9ruI8k0P3pMSHPnpfcxXHECiE4DBHBffyHrWzJiD1NnRJwwdeSuxVwi15GA/+8yOi6/ojk33bNkqs+TqYHIArBOne3JA4My3iBe4FNbSpWTpqYVXfCk7trv8EA81Jdy/2f1qvBevGZVfJELGbfiCM8SZ8BYBnPlTWJJHgB9vpmXuO9DXlaZ3VByW2ZJR3Wq5IrtIzWtC6V44cvrujOCvrv/Y3hBr9RxoK55MAAAAASUVORK5CYII=";

        public WidgetWindow()
        {
            Title = "CloudTech Widget";
            Width = 300;
            Height = 390;
            WindowStyle = WindowStyle.None;
            AllowsTransparency = true;
            Background = System.Windows.Media.Brushes.Transparent;
            Topmost = true;
            ShowInTaskbar = false;
            WindowStartupLocation = WindowStartupLocation.Manual;

            // 画面右上に初期配置
            var workArea = SystemParameters.WorkArea;
            Left = workArea.Right - Width - 30;
            Top = workArea.Top + 40;

            BuildUI();
            SetupTrayIcon();

            // ウィンドウ全体をドラッグ可能にする
            MouseLeftButtonDown += (s, e) => {
                if (e.ButtonState == MouseButtonState.Pressed)
                {
                    DragMove();
                }
            };
        }

        private BitmapImage LoadLogoBitmap()
        {
            try
            {
                byte[] bytes = Convert.FromBase64String(LogoBase64);
                using (var ms = new MemoryStream(bytes))
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.StreamSource = ms;
                    bitmap.EndInit();
                    bitmap.Freeze();
                    return bitmap;
                }
            }
            catch
            {
                return null;
            }
        }

        private void BuildUI()
        {
            // 外側ドロップシャドウ付きBorder
            var outerBorder = new Border
            {
                CornerRadius = new CornerRadius(16),
                Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(253, 252, 250)),
                BorderBrush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(224, 224, 224)),
                BorderThickness = new Thickness(1),
                Effect = new DropShadowEffect
                {
                    Color = Colors.Black,
                    Direction = 270,
                    ShadowDepth = 5,
                    Opacity = 0.25,
                    BlurRadius = 15
                },
                Margin = new Thickness(8)
            };

            var mainPanel = new StackPanel();

            // 1. ヘッダーバー (ネイビー背景)
            var headerBorder = new Border
            {
                CornerRadius = new CornerRadius(15, 15, 0, 0),
                Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(35, 47, 62)),
                Padding = new Thickness(12, 10, 12, 10)
            };

            var headerGrid = new Grid();
            headerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            headerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            // タイトル
            var titlePanel = new StackPanel { Orientation = Orientation.Horizontal };
            
            // ロゴ画像 (公式アイコン)
            var logoBmp = LoadLogoBitmap();
            if (logoBmp != null)
            {
                var logoImg = new System.Windows.Controls.Image
                {
                    Source = logoBmp,
                    Width = 22,
                    Height = 22,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(0, 0, 8, 0)
                };
                titlePanel.Children.Add(logoImg);
            }
            else
            {
                var iconLabel = new TextBlock
                {
                    Text = "☁",
                    FontSize = 18,
                    Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 153, 0)),
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(0, 0, 8, 0)
                };
                titlePanel.Children.Add(iconLabel);
            }

            var titleText = new TextBlock
            {
                Text = "CloudTech",
                FontSize = 15,
                FontWeight = FontWeights.Bold,
                Foreground = System.Windows.Media.Brushes.White,
                VerticalAlignment = VerticalAlignment.Center
            };
            titlePanel.Children.Add(titleText);
            Grid.SetColumn(titlePanel, 0);
            headerGrid.Children.Add(titlePanel);

            // 操作ボタン群 (最前面ピン, 最小化, 閉じる)
            var buttonsPanel = new StackPanel { Orientation = Orientation.Horizontal };

            pinText = new TextBlock
            {
                Text = "📌",
                FontSize = 12,
                Foreground = System.Windows.Media.Brushes.White,
                Cursor = Cursors.Hand,
                Margin = new Thickness(6, 0, 6, 0),
                VerticalAlignment = VerticalAlignment.Center,
                ToolTip = "最前面表示の切り替え"
            };
            pinText.MouseLeftButtonDown += (s, e) => {
                isPinned = !isPinned;
                Topmost = isPinned;
                pinText.Opacity = isPinned ? 1.0 : 0.4;
                e.Handled = true;
            };

            var minText = new TextBlock
            {
                Text = "―",
                FontSize = 13,
                FontWeight = FontWeights.Bold,
                Foreground = System.Windows.Media.Brushes.White,
                Cursor = Cursors.Hand,
                Margin = new Thickness(6, 0, 6, 0),
                VerticalAlignment = VerticalAlignment.Center,
                ToolTip = "タスクトレイに最小化"
            };
            minText.MouseLeftButtonDown += (s, e) => {
                Hide();
                trayIcon.ShowBalloonTip(1500, "CloudTech Widget", "タスクトレイに最小化しました", ToolTipIcon.Info);
                e.Handled = true;
            };

            var closeText = new TextBlock
            {
                Text = "✕",
                FontSize = 13,
                Foreground = System.Windows.Media.Brushes.White,
                Cursor = Cursors.Hand,
                Margin = new Thickness(6, 0, 2, 0),
                VerticalAlignment = VerticalAlignment.Center,
                ToolTip = "終了"
            };
            closeText.MouseLeftButtonDown += (s, e) => {
                Close();
                e.Handled = true;
            };

            buttonsPanel.Children.Add(pinText);
            buttonsPanel.Children.Add(minText);
            buttonsPanel.Children.Add(closeText);
            Grid.SetColumn(buttonsPanel, 1);
            headerGrid.Children.Add(buttonsPanel);

            headerBorder.Child = headerGrid;
            mainPanel.Children.Add(headerBorder);

            // 2. ボタンリスト
            var buttonsContainer = new StackPanel
            {
                Margin = new Thickness(12, 10, 12, 12)
            };

            buttonsContainer.Children.Add(CreateLinkCard("🏠", "CloudTech ホーム", "トップ・マイページ", "https://kws-cloud-tech.com/", System.Windows.Media.Color.FromRgb(255, 153, 0)));
            buttonsContainer.Children.Add(CreateLinkCard("☁", "AWSコース一覧", "実践的カリキュラム", "https://kws-cloud-tech.com/courses/aws-course/", System.Windows.Media.Color.FromRgb(0, 115, 187)));
            buttonsContainer.Children.Add(CreateLinkCard("🎬", "資格対策動画", "認定試験対策の動画講義", "https://kws-cloud-tech.com/lessons/cert-video-intro/", System.Windows.Media.Color.FromRgb(230, 81, 0)));
            buttonsContainer.Children.Add(CreateLinkCard("🎓", "新資格対策コース", "最新認定試験対策", "https://kws-cloud-tech.com/courses/new-cert/", System.Windows.Media.Color.FromRgb(27, 94, 32)));

            mainPanel.Children.Add(buttonsContainer);

            outerBorder.Child = mainPanel;
            Content = outerBorder;
        }

        private Border CreateLinkCard(string icon, string title, string subtitle, string url, System.Windows.Media.Color accentColor)
        {
            var card = new Border
            {
                CornerRadius = new CornerRadius(10),
                Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(244, 246, 249)),
                BorderBrush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(230, 233, 238)),
                BorderThickness = new Thickness(1),
                Padding = new Thickness(10, 8, 10, 8),
                Margin = new Thickness(0, 0, 0, 8),
                Cursor = Cursors.Hand
            };

            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(32) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(20) });

            // アイコン
            var iconBlock = new TextBlock
            {
                Text = icon,
                FontSize = 18,
                Foreground = new SolidColorBrush(accentColor),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center
            };
            Grid.SetColumn(iconBlock, 0);
            grid.Children.Add(iconBlock);

            // テキスト
            var textPanel = new StackPanel { Margin = new Thickness(6, 0, 0, 0) };
            var titleBlock = new TextBlock
            {
                Text = title,
                FontSize = 13,
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(32, 33, 36))
            };
            var subBlock = new TextBlock
            {
                Text = subtitle,
                FontSize = 10,
                Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(95, 99, 104)),
                Margin = new Thickness(0, 1, 0, 0)
            };
            textPanel.Children.Add(titleBlock);
            textPanel.Children.Add(subBlock);
            Grid.SetColumn(textPanel, 1);
            grid.Children.Add(textPanel);

            // 矢印
            var arrowBlock = new TextBlock
            {
                Text = "➔",
                FontSize = 11,
                Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(180, 185, 190)),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right
            };
            Grid.SetColumn(arrowBlock, 2);
            grid.Children.Add(arrowBlock);

            card.Child = grid;

            // ホバーエフェクト
            card.MouseEnter += (s, e) => {
                card.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 248, 235));
                card.BorderBrush = new SolidColorBrush(accentColor);
            };
            card.MouseLeave += (s, e) => {
                card.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(244, 246, 249));
                card.BorderBrush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(230, 233, 238));
            };

            // クリックでGoogle Chromeを優先して起動
            card.MouseLeftButtonDown += (s, e) => {
                OpenInChrome(url);
                e.Handled = true;
            };

            return card;
        }

        private static void OpenInChrome(string url)
        {
            try
            {
                // Chromeの一般的なインストール先パス
                var chromePaths = new[]
                {
                    @"C:\Program Files\Google\Chrome\Application\chrome.exe",
                    @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe",
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Google\Chrome\Application\chrome.exe")
                };

                foreach (var path in chromePaths)
                {
                    if (File.Exists(path))
                    {
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = path,
                            Arguments = "\"" + url + "\"",
                            UseShellExecute = false
                        });
                        return;
                    }
                }

                // PATH上の chrome.exe を試行
                try
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = "chrome.exe",
                        Arguments = "\"" + url + "\"",
                        UseShellExecute = true
                    });
                    return;
                }
                catch { }

                // Chromeが見つからない場合のフォールバック（既定ブラウザ）
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("ブラウザを開けませんでした: " + ex.Message);
            }
        }

        private void SetupTrayIcon()
        {
            trayIcon = new NotifyIcon
            {
                Text = "CloudTech Widget",
                Visible = true
            };

            // トレイアイコンに公式ロゴを適用
            try
            {
                byte[] bytes = Convert.FromBase64String(LogoBase64);
                using (var ms = new MemoryStream(bytes))
                using (var bmp = new Bitmap(ms))
                {
                    IntPtr hIcon = bmp.GetHicon();
                    trayIcon.Icon = System.Drawing.Icon.FromHandle(hIcon);
                }
            }
            catch
            {
                trayIcon.Icon = SystemIcons.Application;
            }

            var contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("ウィジェットを表示", null, (s, e) => {
                Show();
                WindowState = WindowState.Normal;
                Activate();
            });
            contextMenu.Items.Add("終了", null, (s, e) => {
                trayIcon.Visible = false;
                Close();
            });

            trayIcon.ContextMenuStrip = contextMenu;
            trayIcon.DoubleClick += (s, e) => {
                if (Visibility == Visibility.Visible)
                {
                    Hide();
                }
                else
                {
                    Show();
                    WindowState = WindowState.Normal;
                    Activate();
                }
            };
        }

        protected override void OnClosed(EventArgs e)
        {
            if (trayIcon != null)
            {
                trayIcon.Visible = false;
                trayIcon.Dispose();
            }
            base.OnClosed(e);
            Application.Current.Shutdown();
        }
    }
}
