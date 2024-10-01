import globalStyles from '@/styles/globalStyles';
import { Link } from 'expo-router';
import { Pencil } from 'lucide-react-native';
import { Pressable } from 'react-native';

interface EvaluationEditProps {
  evaluationId: number;
}

export function EvaluationEdit({ evaluationId }: EvaluationEditProps) {
  const globalStyle = globalStyles();

  return (
    <>
      <Link
        style={[globalStyle.buttonPrimary, globalStyle.buttonPrimaryIcons]}
        href={`/evaluation/${'edit;' + evaluationId}`}
        asChild
      >
        <Pressable>
          <Pencil size={18} color="white" />
        </Pressable>
      </Link>
    </>
  );
}
