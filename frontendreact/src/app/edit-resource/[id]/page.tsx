import Hero from "@/components/resource/edit/hero/hero"

interface EditResourceProps {
  params: { id: string };
}

export default function EditResource({ params }: EditResourceProps)
{
    console.log("EditResource params:", params);
    return(
        <div>
            <Hero id={params.id} />
        </div>
    )
}