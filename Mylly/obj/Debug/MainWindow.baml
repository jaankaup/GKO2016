   M S B A M L   `   `   `  ����@  <Mylly, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:MyllyMylly  5   	       Mylly.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   %localclr-namespace:Mylly     5   	   -   5          �Name$  Ikkuna��5   $    � Title$ 
MainWindow��5   	   $	��600��6   $	��800��6)   $	��500��5	   	   $	��500��5
   	      ViewModelli � DataContext# [  5   	      Mylly.MyllyViewModel   RegisterCommandBindings 5   	   �� *CommandBindings+ � 	Resources 5        LineProperties �� 
TargetType �� Property �� Value  FullVerticalLine �� BasedOn	 �� RelativeSource
 �� Mode �� AncestorType �� Path  FullHorizontalLine  	RightLine  LeftLine 
 UpLine  DownLine��%�  &	       &	 m     7  &	 �    7  &	 T    7  &	 �    7  &	 f    7  &	 �    7  �� 5   
   " ��6'   .�5      ��  � ��$ 4��60   �� 5       � ��
 �   �6'   5      �� 5   
   8   " ��6S   .�5      �� 
 �@��X1$	 0.0��6#   �� 5       
 �@��X2 6#   �� 	 �� $
 FindAncestor=�$ CanvasL�$ ActualWidth��� 5!      
 �@��Y1 6#   �� 	 �� $
 FindAncestor=�$ CanvasL�$ 
HalfHeight��� 5"      
 �@��Y2 6#   �� 	 �� $
 FindAncestor=�$ CanvasL�$ 
HalfHeight�5#      �� 5%   
   8   " ��6U   .�5&      �� 
 �@��X1 6#   �� 	 �� $
 FindAncestor=�$ CanvasL�$ 	HalfWidth��� 5'      
 �@��X2 6#   �� 	 �� $
 FindAncestor=�$ CanvasL�$ 	HalfWidth��� 5(      
 �@��Y1$ 0��6#   �� 5)      
 �@��Y2 6#   �� 	 �� $
 FindAncestor=�$ CanvasL�$ ActualHeight�5*      �� 5,   
   8   " ��6N   .�5-      �� 
 �@��X1 6#   �� 	 �� $
 FindAncestor=�$ CanvasL�$ 	HalfWidth�5.      �� 50   
   8   " ��6M   .�51      �� 
 �@��X2 6#   �� 	 �� $
 FindAncestor=�$ CanvasL�$ 	HalfWidth�52      �� 54   
   8   " ��6M   .�55      �� 
 �@��Y2 6#   �� 	 �� $
 FindAncestor=�$ CanvasL�$ 
HalfHeight�56      �� 58   
   8   " ��6O   .�59      �� 
 �@��Y1 6#   �� 	 �� $
 FindAncestor=�$ CanvasL�$ 
HalfHeight�5:      5;      .��5>      �  � RowDefinitions 5@   
   �� 5A      $��20 �6   �� 5B      $��* �6   �� 5C      $��40 �6   .+�5G   
   o�  o� 
IsMainMenu . $��0��6!   .@�5H      m� $��_File��6   .@�5I      m�  m� Command$" local:MyllyViewModel.NewGame��6   m� 5J      $  local:MyllyViewModel.Print��6   5M      m� 5U      $��_Tools��6   .@�5V      m� $* $local:MyllyViewModel.SettingsCommand��6   5W      m� 5X      $��About��6   .@�5Y      m� $' !local:MyllyViewModel.AboutCommand��6   m� 5Z      $&  local:MyllyViewModel.HelpCommand��6   5[      5]      � 5_   
   $��1��6    5a        TextBlockStyle3  -System.Windows.Media.Effects.DropShadowEffect   ShadowDepth   	Direction   Color   Opacity   
BlurRadius��%a  &	       �� 5b      " ��6   .�5c      ��  � ��
 �����63   �� 5d       � ��$ Stretch=�6:   �� 5e       � ��$ Stretch=�6<   �� 5f       �@��Padding$ 1,1,1,3q�60   �� 5g       �@J�Effect 5h        5i      $ 1Q�6/   $	 330Q�6?   $ Black��6O   $	 0.8Q�6]   $ 2Q�6k   5k      5l      5m       5n      �� 5o      $��0 �6    �� 5q      $��* �6     � ColumnDefinitions 5s      �� 5u      $��1* �6#   �� 5v      $��4* �6#   �� 5w      $��1* �6#   .+�5      �� -   5         ��Name$ Voittotuuletus��5      $��1��6-   $��1��6=    K� ZIndex$ 1��6J   .+�5�      �� -   5         ��Name$ VoittoTekstiBlock��5�   q   $��60(�6B   $
��Bold"�6P   
 �� Top$ 0��6b   ��6   �� *TheWinner.PlayerName+��5�      �� 5�      " ��6    .�5�      ��  � y�$ Hidden=�6;   �� 5�       � z�VoittajaTransform�� 5�   "   �� 5�   &    �� Angle$ 0Q�66   JuupasAngle��6@   5�       �� Triggers 5�      �� 5�   "    �� Binding 6.   �� *
IsGameOver+ �� Value True6M   .a�5�   &   ��  � y�$ Visible=�6C    N� EnterActions 5�   &   �� 5�   *    �� Source$9 3pack://siteoforigin:,,,/sound/heres-your-winner.wav?�6<   �� 5�   *     ��Name$  tt��6:   .��5�   .   �� .��5�   2   W� ! �� TargetProperty$! (Canvas.Left)�5�   5   " W� From$	" 500X�5�   5   	# W� To$# 0X�6@   $ g� Duration$$ 0:0:1D�6G   % g� AutoReverse% . & g� RepeatBehavior$& Forever��5�   H   ' W� EasingFunction' 5�   6   -  'System.Windows.Media.Animation.SineEase  5�   :   7  1System.Windows.Media.Animation.EasingFunctionBase(   
EasingMode$( EaseIn=�6C   5�   3   W� 5�   2   $! (Canvas.Top)�5�   5   $	" 500X�5�   5   $# 0X�6@   $$ 0:0:3D�6G   % . $& Forever��5�   H   ' 5�   6     5�   :   $( EaseIn=�6C   5�   3   W� 5�   2   $9! 3(TextBlock.RenderTransform).(RotateTransform.Angle)�6B   $	" 0.0X�5�   B   $# 360.0X�6M   $$ 0:0:5D�6X   % .  $& Forever��5�   V   ' 5�   6     5�   :   $( EaseIn=�6C   5�   3   �� 5�   2   $4! .(TextBlock.Foreground).(SolidColorBrush.Color)�5�   6   ) �� From$	) RedX�5�   6   	* �� To$* DarkRedX�6A   $$ 0:0:0.5D�6N   % . $& Forever��5�   I   5�   /   5�   +   + N� ExitActions+ 5�   &   �� 5�   *   , �� BeginStoryboardName$, tt��6<   6W   5�   #   5�      5�      5�      �� 5�      -   5        - ��Name$- 
GameBorder��5�   }   $��1��6   $��1��6"   $��4q�62   
��� ���$��4,4,4,4q�6[   . �� CornerRadius$. 5��6l   ._�5�      �� ��6   �� / �� ElementName$/ Ikkuna��$ DataContext.MVBlocks���5�      �� 5�      .R�5�      A� 0 J� SnapsToDevicePixels0 . 1 � UseLayoutRounding1 .  2 A� Rows2 6*   �� $ GameRowCount�3 A� Columns3 5�   *   �� $ GameColumnCount�5�   *   5�      ��5�      �� 5�      4 �� DataType   Mylly.Block"4  6'   .R�5�        Mylly.MyCanvas  5  Name$5 BlockCanvas��6>   $��0,0,0,0q�6-   ��5�   "   �� 5�   &   $/ Ikkuna��6.   $  DataContext.GameBoardColor�6C   6 �� Mode$6 OneWay=�6e   7 �� UpdateSourceTrigger$7 PropertyChanged=�6s   .+�5�   "   �� 8 ��Name$8 
VasenViiva��6'   #��[ 6;   �� 5�   "   $8 
OikeaViiva��6'   #��[ 6;   �� 5�   "   $8 	YlosViiva��6'   #��[ 6:   �� 5�   "   $8 	AlasViiva��6'   #��[ 6:   A� 5�   "   9 A�Name$9 Palleroinen��6*   ��5�   &   �� 5�   *   " A�60   .�5�   .   ��  � ��
 �   �6E   �� 5�   .    � ��$ 2��6P   �� 5�   .    � ��
 ���� 6G   �� 5�   .    � �� 6G   �� 	 �� $
 FindAncestor=�$ CanvasL�$ ScaleHeight��� 5�   .    � �� 6F   �� 	 �� $
 FindAncestor=�$ CanvasL�$ 
ScaleWidth��� 5�   .    �@��Top 6K   �� 	 �� $
 FindAncestor=�$ CanvasL�$ ScaleHeight��� 5�   .    �@��Left 6L   �� 	 �� $
 FindAncestor=�$ CanvasL�$ 
ScaleWidth��� 5�   .    � z� 5�   2   T� 5�   6   : T� X: 6I   �� 	 �� $
 FindAncestor=�$ local:MyCanvasL�$ 
TranslateX�; T� Y; 5�   I   �� 	 �� $
 FindAncestor=�$ local:MyCanvasL�$ 
TranslateY�5�   /    5�   .   �� 5�   2    6>   �� *IsSelectable+ True6_    5�   6   �� 5�   :   $  tt��6J   .��5�   >   �� .��5�   B   �� $,! &(Ellipse.Fill).(SolidColorBrush.Color)�5�   I   $) BlackX�5�   I   $* DarkSlateGrayX�6V   $$ 0:0:3D�6i   % . $& Forever��5�   \   W� 5�   B   $! (Ellipse.StrokeThickness)�5�   F   $" 0X�5�   F   $# 10X�6O   $$ 0:0:3D�6W   % . $& Forever��5�   Y   5�   ?   5�   ;   + 5�   6   �� 5�   :   $, tt��6L   6g   5�   3   5�   +   < J� InputBindings< 5  &   d� 5  *   = d� MouseAction$= 	LeftClicke�67   > �� Command$/> )local:MyllyViewModel.InsertPieceSelection��5  )   ? �� CommandParameter? 5  )   �� 5	  #      Mylly.GameNappula  5  "   @  Name$@ PeliNappula��64   $y�Hidden=�6I   A   NappulaColorA 6]   �� $ BlockOwner.PlayerColor�B   NappulaTypeB 5  4   �� $ BlockOwner.PieceShapeId���5  4   �� $/ BlockCanvas��$ ActualWidth���5  4   �� $/ BlockCanvas��$ ActualHeight�< 5  &   d� 5  *   $= 	LeftClicke�67   $/> )local:MyllyViewModel.InsertPieceSelection��5  )   ? 5  )   �� 5  #   5     C �� TriggersC 5     �� 5  "   $ False��6O    6.   �� $ LeftWay�.a�5   &   �� D �� 
TargetName$D 
VasenViiva��6-    � y�$ Hidden=�6[   5!  #   �� 5"  "   $ False��6P    6.   �� $ RightWay�.a�5#  &   �� $D 
OikeaViiva��6-    � y�$ Hidden=�6[   5$  #   �� 5%  "   $ False��6M    6.   �� $ UpWay�.a�5&  &   �� $D 	YlosViiva��6-    � y�$ Hidden=�6Z   5'  #   �� 5(  "   $ False��6O    6.   �� $ DownWay�.a�5)  &   �� $D 	AlasViiva��6-    � y�$ Hidden=�6Z   5*  #   �� 5+  "   $ False��6Q    6.   �� $ 	HasObject�.a�5,  &   �� $D Palleroinen��6-    � y�$ Hidden=�6\   5-  #   �� 5.  "   $
 True��6P    6.   �� $ HasPiece�.a�5/  &   �� $D PeliNappula��6-    � y�$ Visible=�6\   50  #   �� 53  "   $
 True��6[    6.   �� $ IsContentSelectable�.a�54  &   �� $D PeliNappula��6-    �@ IsSelectable . 6^   55  #   �� 58  "   $
 True��6Y    6.   �� $ IsContentSelected�.a�59  &   �� $D PeliNappula��6-    �@ 
IsSelected . 6\   5:  #   P� 5=  "   E P� PropertyE �@J�IsMouseOverF P� ValueF . . �5>  &   �� $D Palleroinen��6-    � ��
 � � �6X   5?  #   5A     5C     5D     � 5F     $��1��6    5G     �� 5I     $��1* �6$   �� 5J     $	��10* �6$    5L     .+�5Q     (� $��0��6   .��5R     �� -   5        $ Pelaaja1TextBlock��5R      ��6;   �� $ Player1.PlayerName���6d   �� $ Player1.PlayerColor���5S     �� 5U     " ��6$   # [ 6D    5V  "   �� 5W  &    62   �� *Player1.HasTurn+ True6V    5X  *   �� 5Y  .   $  tt��6>   .��5Z  2   �� .��5[  6   �� $4! .(TextBlock.Background).(SolidColorBrush.Color)�5\  6   $) WhiteX�5]  6   $	* RedX�6C   $$ 0:0:0.5D�6L   % . $& Forever��5^  I   5_  3   5`  /   + 5b  *   �� 5c  .   $, tt��6@   6[   5e  '   5g     5i     5j     �� 5l     $��1��6   ��6,   �� $ Player1Table���5m     �� 5n     .R�5o     A� 0 . 1 .  2 6*   �� *StartingPieceCount+5q  *   5r     ��5t     �� 5x     $4 local:Dummy��6'   .R�5y     (� G (� Stretch$G Uniform=�6&   .��5|  "     $5 P1Canvas��61   $	��Top=�6C   $��50��6X   $��50��6g   .+�5}  &     A 68   �� $% DataContext.Player1.PlayerColor�	 �� " �B 5~  4   �� $&  DataContext.Player1.PieceShapeId�	 �� " ���5  4   �� $/ P1Canvas��$ ActualWidth���5�  4   �� $/ P1Canvas��$ ActualHeight�5�  #   5�     5�     5�     5�     � 5�     $��1��6   $��2��6     5�     �� 5�     $��1* �6$   �� 5�     $	��10* �6$   .+�5�     (� $��0��6   .��5�     �� -   5        $ Pelaaja2TextBlock��5�      ��6;   �� $ Player2.PlayerName���6d   �� $ Player2.PlayerColor���5�     �� 5�     " ��6$   # [ 6D    5�  "   �� 5�  &    62   �� *Player2.HasTurn+ True6V    5�  *   �� 5�  .   $  tt��6>   .��5�  2   �� .��5�  6   �� $4! .(TextBlock.Background).(SolidColorBrush.Color)�5�  6   $) WhiteX�5�  6   $	* RedX�6C   $$ 0:0:0.5D�6L   % . $& Forever��5�  I   5�  3   5�  /   + 5�  *   �� 5�  .   $, tt��6@   6[   5�  '   5�     5�     5�     �� 5�     $��1��6   ��6,   �� $ Player2Table���5�     �� 5�     .R�5�     A� 0 . 1 .  2 6*   �� *StartingPieceCount+5�  *   5�     ��5�     �� 5�     $4 local:Dummy��6'   .R�5�     (� $G Uniform=�6&   .��5�  "     $5 P2Canvas��61   $	��Top=�6C   $��50��6X   $��50��6c   .+�5�  &     A 68   �� $% DataContext.Player2.PlayerColor�	 �� " �B 5�  4   �� $&  DataContext.Player2.PieceShapeId�	 �� " ���5�  4   �� $/ P2Canvas��$ ActualWidth���5�  4   �� $/ P2Canvas��$ ActualHeight�5�  #   5�     5�     5�     5�     5�     (� 5�  
   $��2��6   .��5�     �� $��25(�6   $��Center=�6&   $��Center=�6C   ��6^   �� $ GameMessage�5�     5�     5�     