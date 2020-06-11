import javax.swing.JFrame;
import javax.swing.JTextArea;

public class MainFrame extends JFrame
{
    String[] args = null;
    JTextArea argsList = null;
    
    public MainFrame(String[] args)
    {
        this.args = args;
        
        StringBuilder argsList = new StringBuilder();
        argsList.append("Args passed into the app:\n");
        
        for (int i = 0; i < args.length; i++)
            argsList.append(String.format("%s. %s\n", i + 1, args[i]));
        
        this.argsList = new JTextArea(argsList.toString());
        this.argsList.setEditable(false);
        
        this.setTitle("Test Frame");
        this.setSize(640, 480);
        this.add(this.argsList);
        this.setLocationRelativeTo(null);
        this.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
    }
    
    public void display()
    {
        this.setVisible(true);
    }
}
