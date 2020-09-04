import javax.swing.JFrame;
import javax.swing.JTextArea;
import java.util.List;

public class MainFrame extends JFrame
{
    String[] args = null;
    JTextArea argsList = null;
    
    public MainFrame(String[] args, List<String> vmArgs)
    {
        this.args = args;
        
        StringBuilder argsList = new StringBuilder();
        argsList.append("Args passed into the app:\n");
        
        for (int i = 0; i < args.length; i++)
            argsList.append(String.format("%s. %s\n", i + 1, args[i]));

        argsList.append("\nArgs passed into JVM:\n");
        for (int i = 0; i < vmArgs.size(); i++)
            argsList.append(String.format("%s. %s\n", i + 1, vmArgs.get(i)));
        
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
